using API.DTOs.GameRoom;
using API.Entities.GameRoom;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// GameRoomController contains all the endpoints for Game Room related actions
    /// </summary>
    public class GameRoomController : BaseApiController
    {
        private readonly IGameRoomRepository _gameRoomRepository;
        private readonly IMapper _mapper;
        private readonly ISteamAppsClient _steamAppsClient;
        private readonly ISteamStoreClient _steamStoreClient;
        public GameRoomController(IGameRoomRepository gameRoomRepository, IMapper mapper, ISteamAppsClient steamAppsClient, ISteamStoreClient steamStoreClient)
        {
            _steamStoreClient = steamStoreClient;
            _steamAppsClient = steamAppsClient;
            _mapper = mapper;
            _gameRoomRepository = gameRoomRepository;
        }

        /// <summary>
        /// Post for creating a new Game Room
        /// </summary>
        /// <param name="newGameRoom"></param>
        /// <returns> </returns>
        [HttpPost("create")]
        public async Task<ActionResult<GameRoomDto>> CreateGameRoom(NewGameRoomDto newGameRoom)
        {

            // Create a new GameRoom object
            var gameRoom = new GameRoom
            {
                MaxUsers = newGameRoom.MaxUsers,
                AdminUid = newGameRoom.AdminUid,
                Title = newGameRoom.Title,
                RoomRequirement = new Requirement
                {
                    Gender = newGameRoom.RoomRequirement.Gender
                }
            };

            // Add a new Game Room
            _gameRoomRepository.addGameRoomAsync(gameRoom);

            // Checks the result from adding the new Game Room
            if (await _gameRoomRepository.SaveAllAsync()) return Ok(_mapper.Map<NewGameRoomDto>(gameRoom));

            return BadRequest("Failed to create room");
        }

        [HttpGet("steam-test")]
        public async Task<ActionResult> TestSteamApi()
        {
            var steam = await _steamAppsClient.GetAppsList();
            return Ok();
        }

        [HttpGet("store-test")]
        public async Task<ActionResult> TestStoreApi()
        {
            var steam = await _steamStoreClient.GetAppInfo(872200);
            Console.WriteLine(steam.Data.About_the_game);
            return Ok();
        }

    }
}