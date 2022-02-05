using API.DTOs;
using API.DTOs.Lobby;
using API.Entities.Lobby;
using API.Entities.Users;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>();
            CreateMap<Lobby, LobbyDto>();
            CreateMap<Lobby, NewLobbyDto>();
            CreateMap<Lobby, Requirement>();
            CreateMap<Requirement, RequirementDto>();
        }
    }
}