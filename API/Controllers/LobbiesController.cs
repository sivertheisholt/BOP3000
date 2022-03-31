using API.DTOs.Lobbies;
using API.Entities.Lobbies;
using API.Interfaces.IRepositories;
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
        private readonly ILobbiesRepository _lobbiesRepository;
        private readonly IUserRepository _userRepository;
        private readonly LobbyHub _lobbyHub;
        private readonly LobbyChatHub _lobbyChatHub;
        private readonly LobbyTracker _lobbyTracker;
        private readonly ISteamAppRepository _steamAppRepository;
        public LobbiesController(ILobbiesRepository lobbiesRepository, IUserRepository userRepository, IMapper mapper, LobbyHub lobbyHub, LobbyTracker lobbyTracker, LobbyChatHub lobbyChatHub, ISteamAppRepository steamAppRepository) : base(mapper)
        {
            _steamAppRepository = steamAppRepository;
            _lobbyTracker = lobbyTracker;
            _lobbyChatHub = lobbyChatHub;
            _lobbyHub = lobbyHub;
            _userRepository = userRepository;
            _lobbiesRepository = lobbiesRepository;
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
            // Create a new GameRoom object
            var lobby = new Lobby
            {
                AdminUid = GetUserIdFromClaim(),
                MaxUsers = newLobby.MaxUsers,
                Title = newLobby.Title,
                LobbyDescription = newLobby.LobbyDescription,
                GameId = newLobby.GameId,
                GameName = (await _steamAppRepository.GetAppInfoAsync(newLobby.GameId)).Data.Name,
                GameType = newLobby.GameType,
                LobbyRequirement = new Requirement
                {
                    Gender = newLobby.LobbyRequirement.Gender
                },
                StartDate = DateTime.Now
            };

            // Add a new Game Room
            _lobbiesRepository.AddLobby(lobby);

            // Checks the result from adding the new Game Room
            if (await _lobbiesRepository.SaveAllAsync())
            {
                var createdLobby = Mapper.Map<NewLobbyDto>(lobby);
                await _lobbyHub.CreateLobby(createdLobby.Id, GetUserIdFromClaim());
                await _lobbyChatHub.CreateChat(createdLobby.Id);
                return Ok(createdLobby);
            }

            return BadRequest("Failed to create room");
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<LobbyDto>> GetLobby(int id)
        {
            var lobby = await _lobbiesRepository.GetLobbyAsync(id);

            if (lobby == null) return NotFound();

            var lobbyDto = Mapper.Map<LobbyDto>(lobby);

            if (!lobby.Finished)
            {
                lobbyDto.Users = await _lobbyTracker.GetMembersInLobby(id);
            }

            var lobbyAdmin = await _userRepository.GetUserByIdAsync(lobby.AdminUid);
            lobbyDto.AdminUsername = lobbyAdmin.UserName;


            return lobbyDto;
        }

        [HttpGet("")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> GetLobbies()
        {
            var lobbies = await _lobbiesRepository.GetLobbiesAsync();

            if (lobbies == null) return NotFound();

            var lobbiesDto = Mapper.Map<List<LobbyDto>>(lobbies);

            foreach (var lobby in lobbiesDto)
            {
                var lobbyAdmin = await _userRepository.GetUserByIdAsync(lobby.AdminUid);

                if (lobby.Finished)
                {
                    lobby.Users = await _lobbyTracker.GetMembersInLobby(lobby.Id);
                }

                lobby.AdminUsername = lobbyAdmin.UserName;
            }
            return lobbiesDto;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("game/{id}")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> GetLobbiesWithGameId(int id)
        {
            var lobbies = await _lobbiesRepository.GetLobbiesWithGameId(id);

            var lobbiesDto = Mapper.Map<List<LobbyDto>>(lobbies);

            foreach (var lobby in lobbiesDto)
            {
                var lobbyAdmin = await _userRepository.GetUserByIdAsync(lobby.AdminUid);

                if (!lobby.Finished)
                {
                    lobby.Users = await _lobbyTracker.GetMembersInLobby(lobby.Id);
                }

                lobby.AdminUsername = lobbyAdmin.UserName;
            }

            return lobbiesDto;
        }
        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}/finished")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> IsLobbyFinished(int id)
        {
            var lobby = await _lobbiesRepository.GetLobbyAsync(id);
            return Ok(lobby.Finished);
        }
    }
}