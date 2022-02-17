using API.DTOs.Members;
using API.DTOs.Lobbies;
using API.DTOs.SteamApps;
using API.Entities.Lobbies;
using API.Entities.SteamApps;
using API.Entities.Users;
using AutoMapper;
using API.Entities.Countries;
using API.DTOs.Countries;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(
                    dest => dest.MemberProfile,
                    opt => opt.MapFrom(src => src.AppUserProfile));
            CreateMap<Lobby, LobbyDto>();
            CreateMap<Lobby, NewLobbyDto>();
            CreateMap<Lobby, Requirement>();
            CreateMap<Requirement, RequirementDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<AppListInfo, AppListInfoDto>();

            CreateMap<AppUserProfile, MemberProfileDto>()
                .ForMember(
                    dest => dest.MemberData,
                    opt => opt.MapFrom(src => src.AppUserData));
            CreateMap<MemberProfileDto, AppUserProfile>().ReverseMap()
                .ForMember(
                    dest => dest.MemberData,
                    opt => opt.MapFrom(src => src.AppUserData));

            CreateMap<AppUserData, MemberDataDto>();
            CreateMap<MemberDataDto, AppUserData>().ReverseMap();

            CreateMap<CountryIso, CountryIsoDto>();
            CreateMap<CountryIsoDto, CountryIso>().ReverseMap();
        }
    }
}