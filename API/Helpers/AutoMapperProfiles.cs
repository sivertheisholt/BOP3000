using API.DTOs;
using API.DTOs.Lobbies;
using API.DTOs.SteamApps;
using API.Entities.Lobbies;
using API.Entities.SteamApps;
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
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<AppListInfo, AppListInfoDto>();
        }
    }
}