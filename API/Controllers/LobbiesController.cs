using System.Security.Claims;
using API.DTOs.Lobbies;
using API.Entities.Lobbies;
using API.Interfaces.IRepositories;
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
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public LobbiesController(ILobbiesRepository lobbiesRepository, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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
            if (await _lobbiesRepository.SaveAllAsync()) return Ok(_mapper.Map<NewLobbyDto>(lobby));

            return BadRequest("Failed to create room");
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<LobbyDto>> GetLobby(int id)
        {
            var lobby = await _lobbiesRepository.GetLobbyAsync(id);

            if (lobby == null) return NotFound();

            return _mapper.Map<LobbyDto>(lobby);
        }

        [HttpGet("")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<LobbiesListDto>> GetLobbies()
        {
            var lobbies = await _lobbiesRepository.GetLobbiesAsync();
            if (lobbies == null) return NotFound();

            return _mapper.Map<LobbiesListDto>(lobbies);
        }
    }
}