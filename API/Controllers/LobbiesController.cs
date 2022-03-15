using System.Security.Claims;
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
        private readonly LobbyChatTracker _lobbyChatTracker;
        private readonly LobbyHub _lobbyHub;
        public LobbiesController(ILobbiesRepository lobbiesRepository, IUserRepository userRepository, IMapper mapper, LobbyChatTracker lobbyChatTracker, LobbyHub lobbyHub) : base(mapper)
        {
            _lobbyHub = lobbyHub;
            _lobbyChatTracker = lobbyChatTracker;
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
                GameType = newLobby.GameType,
                LobbyRequirement = new Requirement
                {
                    Gender = newLobby.LobbyRequirement.Gender
                }
            };

            // Add a new Game Room
            _lobbiesRepository.AddLobby(lobby);

            // Checks the result from adding the new Game Room
            if (await _lobbiesRepository.SaveAllAsync())
            {
                var createdLobby = Mapper.Map<NewLobbyDto>(lobby);
                await _lobbyHub.CreateLobby(createdLobby.Id, GetUserIdFromClaim());
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

            return Mapper.Map<LobbyDto>(lobby);
        }

        [HttpGet("")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> GetLobbies()
        {
            var lobbies = await _lobbiesRepository.GetLobbiesAsync();
            if (lobbies == null) return NotFound();

            return Ok(Mapper.Map<List<LobbyDto>>(lobbies));
        }

        [HttpGet("join")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult> JoinLobby(string lobbyId)
        {
            var userId = GetUserIdFromClaim();

            if (!await _lobbiesRepository.AddPlayerToLobby(userId)) return BadRequest("Could not join the room");

            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("game/{id}")]
        public async Task<ActionResult<IEnumerable<LobbyDto>>> GetLobbiesWithGameId(int id)
        {
            var lobbies = await _lobbiesRepository.GetLobbiesWithGameId(id);

            return Ok(Mapper.Map<List<LobbyDto>>(lobbies));
        }
    }
}