using API.DTOs.Activities;
using API.DTOs.Applications;
using API.DTOs.Members;
using API.Entities.Applications;
using API.Entities.Users;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using API.SignalR;
using AutoMapper;
using Meilisearch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MembersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly LobbyTracker _lobbyTracker;
        private readonly IMeilisearchService _meilisearchService;
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly IPhotoService _photoService;
        public MembersController(IUserRepository userRepository, IMapper mapper, ICountryRepository countryRepository, LobbyTracker lobbyTracker, IMeilisearchService meilisearchService, IActivitiesRepository activitiesRepository, IPhotoService photoService) : base(mapper)
        {
            _photoService = photoService;
            _activitiesRepository = activitiesRepository;
            _meilisearchService = meilisearchService;
            _lobbyTracker = lobbyTracker;
            _countryRepository = countryRepository;
            _userRepository = userRepository;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMember(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) return NotFound();

            return Ok(Mapper.Map<MemberDto>(user));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
        {
            var users = await _userRepository.GetUsersAsync();

            if (users == null) return NotFound();

            return Ok(Mapper.Map<IEnumerable<MemberDto>>(users));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("")]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var userId = GetUserIdFromClaim();

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            var member = Mapper.Map(memberUpdateDto, user);
            member.AppUserProfile.CountryIso = await _countryRepository.GetCountryIsoByIdAsync(memberUpdateDto.CountryId);
            member.AppUserProfile.Birthday = DateTime.Parse(memberUpdateDto.Birthday);
            member.AppUserProfile.Gender = memberUpdateDto.Gender;
            member.AppUserProfile.Description = memberUpdateDto.Description;

            _userRepository.Update(member);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("current")]
        public async Task<ActionResult<MemberDto>> GetCurrentUser()
        {
            var userId = GetUserIdFromClaim();

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            return Ok(Mapper.Map<MemberDto>(user));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("current/activity")]
        public async Task<ActionResult<IEnumerable<MemberMeiliDto>>> GetCurrentUserActivityLog(string name, int limit = 10)
        {
            var uid = GetUserIdFromClaim();

            var followers = await _userRepository.GetUserFollowers(uid);

            var activities = new List<ActivityLogDto>();

            foreach (var follower in followers)
            {

                var activites = await _activitiesRepository.GetActivitiesForUser(follower);
                var activitiesDto = Mapper.Map<List<ActivityLogDto>>(activites);

                foreach (var activityDto in activitiesDto)
                {
                    activityDto.Username = await _userRepository.GetUsernameFromId(activityDto.AppUserId);
                    activities.Add(activityDto);
                }
            }

            activities.Sort((y, x) => x.Date.CompareTo(y.Date));
            return Ok(activities);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("lobby-status")]
        public async Task<ActionResult> GetLobbyStatus()
        {
            var userId = GetUserIdFromClaim();
            var status = new MemberLobbyStatusDto
            {
                InQueue = false,
                LobbyId = 0
            };

            if (!await _lobbyTracker.CheckIfMemberInAnyLobby(userId)) return Ok(status);
            var lobbyId = await _lobbyTracker.GetLobbyIdFromUser(userId);

            status.InQueue = await _lobbyTracker.CheckIfMemberInQueue(lobbyId, userId);
            status.LobbyId = lobbyId;

            return Ok(status);
        }


        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("follow")]
        public async Task<ActionResult> FollowMember(int memberId)
        {
            var userId = GetUserIdFromClaim();
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            if (!await _userRepository.CheckIfUserExists(memberId)) return NotFound("Member you are trying to follow doesn't exist");

            if (user.AppUserProfile.AppUserData.Following.Contains(memberId)) return NoContent();

            user.AppUserProfile.AppUserData.Following.Add(memberId);
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("unfollow")]
        public async Task<ActionResult> UnFollowMember(int memberId)
        {
            var userId = GetUserIdFromClaim();
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            if (!await _userRepository.CheckIfUserExists(memberId)) return NotFound("Member you are trying to follow doesn't exist");

            if (!user.AppUserProfile.AppUserData.Following.Contains(memberId)) return NoContent();

            user.AppUserProfile.AppUserData.Following.Remove(memberId);
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("check-follow")]
        public async Task<ActionResult<bool>> CheckIfFollowed(int memberId)
        {
            var userId = GetUserIdFromClaim();
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            if (user.AppUserProfile.AppUserData.Following.Contains(memberId)) return Ok(true);

            return Ok(false);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MemberMeiliDto>>> SearchForMember(string name, int limit = 10)
        {
            var index = _meilisearchService.GetIndex("members");
            var hits = await _meilisearchService.SearchAsync<AppUserMeili>(index, name, new SearchQuery { Limit = limit });
            return Ok(Mapper.Map<IEnumerable<AppUserMeili>>(hits.Hits));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPost("set-photo")]
        public async Task<ActionResult<MemberPhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new AppUserPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            user.AppUserProfile.AppUserPhoto = photo;

            if (await _userRepository.SaveAllAsync()) return Mapper.Map<MemberPhotoDto>(photo);

            return BadRequest("Problem adding photo");
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}/discord")]
        public async Task<ActionResult<DiscordStatusDto>> DiscordConnection(int id)
        {
            var user = await _userRepository.GetUserConnectionsFromUid(id);

            if (user == null) return NotFound();

            var dto = new DiscordStatusDto
            {
                DiscordId = user.Discord.DiscordId,
                Connected = user.DiscordConnected,
                Username = user.Discord.Username,
                Discriminator = user.Discord.Distriminator,
                Hidden = user.Discord.Hidden
            };
            return dto;
        }
        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}/steam")]
        public async Task<ActionResult<SteamStatusDto>> SteamConnection(int id)
        {
            var user = await _userRepository.GetUserConnectionsFromUid(id);

            if (user == null) return NotFound();

            var dto = new SteamStatusDto
            {
                Connected = user.SteamConnected,
                SteamId = user.Steam.SteamId,
                Hidden = user.Steam.Hidden
            };
            return dto;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("block/{id}")]
        public async Task<ActionResult<SteamStatusDto>> BlockMember(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();
            if (user.AppUserProfile.BlockedUsers == null) user.AppUserProfile.BlockedUsers = new List<int>();
            user.AppUserProfile.BlockedUsers.Add(id);
            _userRepository.Update(user);
            await _userRepository.SaveAllAsync();

            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("discord/unlink")]
        public async Task<ActionResult<SteamStatusDto>> UnlinkDiscord()
        {
            var user = await _userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();

            user.AppUserProfile.UserConnections.Discord = new DiscordProfile { };
            user.AppUserProfile.UserConnections.DiscordConnected = false;

            _userRepository.Update(user);
            await _userRepository.SaveAllAsync();

            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("steam/unlink")]
        public async Task<ActionResult<SteamStatusDto>> UnlinkSteam()
        {
            var user = await _userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();

            user.AppUserProfile.UserConnections.Steam = new SteamProfile { };
            user.AppUserProfile.UserConnections.SteamConnected = false;

            _userRepository.Update(user);
            await _userRepository.SaveAllAsync();

            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("steam/hide")]
        public async Task<ActionResult<SteamStatusDto>> HideSteam(bool hide)
        {
            var user = await _userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();

            user.AppUserProfile.UserConnections.Steam.Hidden = hide;

            _userRepository.Update(user);
            await _userRepository.SaveAllAsync();

            return NoContent();
        }
        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("discord/hide")]
        public async Task<ActionResult<SteamStatusDto>> HideDiscord(bool hide)
        {
            var user = await _userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();

            user.AppUserProfile.UserConnections.Discord.Hidden = hide;

            _userRepository.Update(user);
            await _userRepository.SaveAllAsync();

            return NoContent();
        }
    }
}