using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Repositories;
using API.DTOs;
using API.DTOs.GameRoom;
using API.Entities.GameRoom;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GameRoomController : BaseApiController
    {
        private readonly IGameRoomRepository _gameRoomRepository;
        private readonly IMapper _mapper;
        public GameRoomController(IGameRoomRepository gameRoomRepository, IMapper mapper)
        {
            _mapper = mapper;
            _gameRoomRepository = gameRoomRepository;
        }

        [HttpPost("create")]
        public async Task<ActionResult<GameRoomDto>> CreateGameRoom(NewGameRoomDto newGameRoom)
        {
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

            _gameRoomRepository.addGameRoomAsync(gameRoom);

            if (await _gameRoomRepository.SaveAllAsync()) return Ok(_mapper.Map<NewGameRoomDto>(gameRoom));

            return BadRequest("Failed to create room");
        }

    }
}