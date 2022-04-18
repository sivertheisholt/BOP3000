using API.DTOs.Members;
using API.DTOs.Lobbies;
using API.DTOs.SteamApps;
using API.Entities.Lobbies;
using API.Entities.SteamApps;
using API.Entities.Users;
using AutoMapper;
using API.Entities.Countries;
using API.DTOs.Countries;
using API.Entities.SteamApp;
using API.DTOs.GameApps;
using API.Entities.SteamApp.Information;
using API.Entities.Activities;
using API.DTOs.Activities;

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
            CreateMap<LobbyVote, LobbyVoteDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<AppListInfo, AppListInfoDto>();

            CreateMap<AppUserProfile, MemberProfileDto>()
                .ForMember(
                    dest => dest.MemberData,
                    opt => opt.MapFrom(src => src.AppUserData))
                .ForMember(
                    dest => dest.MemberPhoto,
                    opt => opt.MapFrom(src => src.AppUserPhoto));

            CreateMap<MemberProfileDto, AppUserProfile>().ReverseMap()
                .ForMember(
                    dest => dest.MemberData,
                    opt => opt.MapFrom(src => src.AppUserData))
                .ForMember(
                    dest => dest.MemberPhoto,
                    opt => opt.MapFrom(src => src.AppUserPhoto));

            CreateMap<AppUserData, MemberDataDto>();
            CreateMap<MemberDataDto, AppUserData>().ReverseMap();

            CreateMap<CountryIso, CountryIsoDto>();
            CreateMap<CountryIsoDto, CountryIso>().ReverseMap();

            CreateMap<AppData, GameAppInfoDto>();

            CreateMap<Message, MessageDto>();

            CreateMap<AppUser, AppUserMeili>();

            CreateMap<ActivityLog, ActivityLogDto>()
                .ForMember(dest => dest.Identifier,
                opt => opt.MapFrom(src => src.Activity.Identifier));

            CreateMap<AppUserPhoto, MemberPhotoDto>();

            CreateMap<Log, LogDto>();
        }
    }
}