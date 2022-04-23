using API.DTOs.Lobbies;
using API.Entities.Activities;
using API.Entities.Lobbies;
using API.Extentions;
using API.Helpers.PaginationsParams;
using API.Interfaces;
using API.Interfaces.IClients;
using API.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// GameRoomController contains all the endpoints for Game Room related actions
    /// </summary>
    public class LobbiesController : BaseApiController
    {
        private readonly LobbyHub _lobbyHub;
        private readonly LobbyChatHub _lobbyChatHub;
        private readonly LobbyTracker _lobbyTracker;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISteamStoreClient _steamStoreClient;
        public LobbiesController(IMapper mapper, LobbyHub lobbyHub, LobbyTracker lobbyTracker, LobbyChatHub lobbyChatHub, IUnitOfWork unitOfWork, ISteamStoreClient steamStoreClient) : base(mapper)
        {
            _steamStoreClient = steamStoreClient;
            _unitOfWork = unitOfWork;
            _lobbyTracker = lobbyTracker;
            _lobbyChatHub = lobbyChatHub;
            _lobbyHub = lobbyHub;
        }

        /// <summary>
        /// Post for creating a new Game Room
        /// </summary>
        /// <param name="newGameRoom"></param>
        /// <returns> </returns>
        [HttpPost("")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<LobbyDto>> CreateLobby(NewLobbyDto newLobby)
        {
            // Create a new GameRoom objec
            var game = await _unitOfWork.steamAppRepository.GetAppInfoBySteamId(newLobby.GameId);
            if (game == null)
            {
                game = await _steamStoreClient.GetAppInfo(newLobby.GameId);
                _unitOfWork.steamAppRepository.AddApp(game);
                await _unitOfWork.Complete();
            }
            var lobby = new Lobby
            {
                AdminUid = GetUserIdFromClaim(),
                MaxUsers = newLobby.MaxUsers,
                Title = newLobby.Title,
                LobbyDescription = newLobby.LobbyDescription,
                GameId = game.Id,
                GameName = game.Data.Name,
                GameType = newLobby.GameType,
                StartDate = DateTime.Now
            };

            // Add a new Game Room
            _unitOfWork.lobbiesRepository.AddLobby(lobby);
            await _unitOfWork.Complete();

            // Checks the result from adding the new Game Room
            var createdLobby = Mapper.Map<NewLobbyDto>(lobby);
            await _lobbyHub.CreateLobby(lobby, GetUserIdFromClaim());
            await _lobbyChatHub.CreateChat(createdLobby.Id);

            var activityLog = new ActivityLog
            {
                Date = DateTime.Now,
                LobbyId = createdLobby.Id,
                AppUserId = GetUserIdFromClaim(),
                ActivityId = 2,
            };

            _unitOfWork.activitiesRepository.AddActivityLog(activityLog);

            if (!await _unitOfWork.Complete()) return BadRequest("Could not create lobby");

            return Ok(createdLobby);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<LobbyDto>> GetLobby(int id)
        {
            var lobby = await _unitOfWork.lobbiesRepository.GetLobbyAsync(id);

            if (lobby == null) return NotFound();

            var lobbyDto = Mapper.Map<LobbyDto>(lobby);

            if (!lobby.Finished)
            {
                lobbyDto.Users = _lobbyTracker.GetMembersInLobby(id);
            }

            var lobbyAdmin = await _unitOfWork.userRepository.GetUserByIdAsync(lobby.AdminUid);
            lobbyDto.AdminUsername = lobbyAdmin.UserName;
            lobbyDto.AdminProfilePic = lobbyAdmin.AppUserProfile.AppUserPhoto.Url;

            return lobbyDto;
        }

        [HttpGet("")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> GetLobbies([FromQuery] UniversalParams universalParams)
        {
            var lobbies = await _unitOfWork.lobbiesRepository.GetLobbiesAsync(universalParams);

            Response.AddPaginationHeader(lobbies.CurrentPage, lobbies.PageSize, lobbies.TotalCount, lobbies.TotalPages);

            if (lobbies == null) return NotFound();

            var lobbiesDto = Mapper.Map<List<LobbyDto>>(lobbies);

            foreach (var lobby in lobbiesDto)
            {
                var lobbyAdmin = await _unitOfWork.userRepository.GetUserByIdAsync(lobby.AdminUid);

                if (lobby.Finished)
                {
                    lobby.Users = _lobbyTracker.GetMembersInLobby(lobby.Id);
                }

                lobby.AdminUsername = lobbyAdmin.UserName;
                lobby.AdminProfilePic = lobbyAdmin.AppUserProfile.AppUserPhoto.Url;
            }
            return lobbiesDto;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("game/{id}")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> GetActiveLobbiesWithGameId(int id, [FromQuery] UniversalParams universalParams)
        {
            var lobbies = await _unitOfWork.lobbiesRepository.GetLobbiesWithGameId(id, universalParams);

            Response.AddPaginationHeader(lobbies.CurrentPage, lobbies.PageSize, lobbies.TotalCount, lobbies.TotalPages);

            var lobbiesDto = new List<LobbyDto>();

            foreach (var lobby in lobbies)
            {
                if (lobby.Finished) continue;

                var lobbyDto = Mapper.Map<LobbyDto>(lobby);
                var lobbyAdmin = await _unitOfWork.userRepository.GetUserByIdAsync(lobby.AdminUid);

                lobbyDto.AdminUsername = lobbyAdmin.UserName;
                lobbyDto.AdminProfilePic = lobbyAdmin.AppUserProfile.AppUserPhoto.Url;
                lobbyDto.Users = _lobbyTracker.GetMembersInLobby(lobby.Id);
                lobbiesDto.Add(lobbyDto);
            }

            return lobbiesDto;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}/finished")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> IsLobbyFinished(int id)
        {
            var lobby = await _unitOfWork.lobbiesRepository.GetLobbyAsync(id);
            return Ok(lobby.Finished);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("{id}/upvote/{uid}")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> Upvote(int id, int uid)
        {
            var lobby = await _unitOfWork.lobbiesRepository.GetLobbyAsync(id);

            if (!lobby.Finished) return BadRequest("Lobby is not finished");

            if (!lobby.Users.Contains(GetUserIdFromClaim())) return BadRequest("You are not in this lobby");

            var userVotes = lobby.Votes.Where(vote => vote.VoterUid == GetUserIdFromClaim()).ToList();

            if (userVotes.FirstOrDefault(vote => vote.VotedUid == uid) != null) return BadRequest("Already vote on this person");

            lobby.Votes.Add(
                new LobbyVote
                {
                    Upvote = true,
                    VotedUid = uid,
                    VoterUid = GetUserIdFromClaim()
                });

            _unitOfWork.lobbiesRepository.Update(lobby);
            if (!await _unitOfWork.Complete()) return StatusCode(StatusCodes.Status500InternalServerError);

            var upvotedUser = await _unitOfWork.userRepository.GetUserByIdAsync(uid);

            upvotedUser.AppUserProfile.AppUserData.Upvotes++;
            _unitOfWork.userRepository.Update(upvotedUser);
            if (!await _unitOfWork.Complete()) return StatusCode(StatusCodes.Status500InternalServerError);

            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("{id}/downvote/{uid}")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> Downvote(int id, int uid)
        {
            var lobby = await _unitOfWork.lobbiesRepository.GetLobbyAsync(id);

            if (!lobby.Finished) return BadRequest("Lobby is not finished");

            if (!lobby.Users.Contains(GetUserIdFromClaim())) return BadRequest("You are not in this lobby");

            var userVotes = lobby.Votes.Where(vote => vote.VoterUid == GetUserIdFromClaim()).ToList();

            if (userVotes.FirstOrDefault(vote => vote.VotedUid == uid) != null) return BadRequest("Already vote on this person");

            lobby.Votes.Add(
                new LobbyVote
                {
                    Upvote = false,
                    VotedUid = uid,
                    VoterUid = GetUserIdFromClaim()
                });

            _unitOfWork.lobbiesRepository.Update(lobby);
            if (!await _unitOfWork.Complete()) return StatusCode(StatusCodes.Status500InternalServerError);

            var upvotedUser = await _unitOfWork.userRepository.GetUserByIdAsync(uid);

            upvotedUser.AppUserProfile.AppUserData.Downvotes++;
            _unitOfWork.userRepository.Update(upvotedUser);
            if (!await _unitOfWork.Complete()) return StatusCode(StatusCodes.Status500InternalServerError);

            return NoContent();
        }
    }
}