using API.DTOs;
using API.DTOs.GameRoom;
using API.Entities.GameRoom;
using API.Entities.Users;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>();
            CreateMap<GameRoom, GameRoomDto>();
            CreateMap<GameRoom, NewGameRoomDto>();
            CreateMap<GameRoom, Requirement>();
            CreateMap<Requirement, RequirementDto>();
        }
    }
}