using System.Reflection;
using API.DTOs.Activities;
using API.DTOs.Applications;
using API.DTOs.Members;
using API.Entities.Activities;
using API.Entities.Applications;
using API.Entities.Users;
using API.Extentions;
using API.Helpers.PaginationsParams;
using API.Interfaces;
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
        private readonly LobbyTracker _lobbyTracker;
        private readonly IMeilisearchService _meilisearchService;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscordBotService _discordBotService;
        public MembersController(IMapper mapper, LobbyTracker lobbyTracker, IMeilisearchService meilisearchService, IPhotoService photoService, IUnitOfWork unitOfWork, IDiscordBotService discordBotService) : base(mapper)
        {
            _discordBotService = discordBotService;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _meilisearchService = meilisearchService;
            _lobbyTracker = lobbyTracker;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMember(int id)
        {
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(id);

            if (user == null) return NotFound();

            return Ok(Mapper.Map<MemberDto>(user));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers([FromQuery] MemberParams memberParams)
        {
            var users = await _unitOfWork.userRepository.GetAllUsers(memberParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            if (users.FirstOrDefault() == null) return NotFound();

            return Ok(Mapper.Map<IEnumerable<MemberDto>>(users));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("")]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var userId = GetUserIdFromClaim();

            var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            if (memberUpdateDto.Birthday == null) return BadRequest("Birthday can't be null");
            if (memberUpdateDto.CountryId == 0) return BadRequest("Country can't be null");
            if (memberUpdateDto.Description == null) return BadRequest("Description can't be null");
            if (memberUpdateDto.Email == null) return BadRequest("Email can't be null");
            if (memberUpdateDto.Gender == null) return BadRequest("Gender can't be null");
            if (memberUpdateDto.UserName == null) return BadRequest("Username can't be null");

            var member = Mapper.Map(memberUpdateDto, user);
            member.AppUserProfile.CountryIso = await _unitOfWork.countryRepository.GetCountryIsoByIdAsync(memberUpdateDto.CountryId);
            member.AppUserProfile.Birthday = DateTime.Parse(memberUpdateDto.Birthday);
            member.AppUserProfile.Gender = memberUpdateDto.Gender;
            member.AppUserProfile.Description = memberUpdateDto.Description;

            _unitOfWork.userRepository.Update(member);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("current")]
        public async Task<ActionResult<MemberDto>> GetCurrentUser()
        {
            var userId = GetUserIdFromClaim();

            var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            return Ok(Mapper.Map<MemberDto>(user));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("current/activity")]
        public async Task<ActionResult<IEnumerable<MemberMeiliDto>>> GetCurrentUserActivityLog()
        {
            var uid = GetUserIdFromClaim();

            var followers = await _unitOfWork.userRepository.GetUserFollowing(uid);

            var activities = new List<ActivityLogDto>();

            foreach (var follower in followers)
            {

                var activitesFollower = await _unitOfWork.activitiesRepository.GetActivitiesForUser(follower);
                var activitiesDto = Mapper.Map<List<ActivityLogDto>>(activitesFollower);

                foreach (var activityDto in activitiesDto)
                {
                    var user = await _unitOfWork.userRepository.GetUserByIdAsync(activityDto.AppUserId);
                    activityDto.Username = user.UserName;
                    activityDto.ProfilePicture = user.AppUserProfile.AppUserPhoto.Url;

                    if (activityDto.LobbyId != 0)
                    {
                        var lobby = await _unitOfWork.lobbiesRepository.GetLobbyAsync(activityDto.LobbyId);
                        var game = await _unitOfWork.steamAppRepository.GetAppInfoAsync(lobby.GameId);
                        activityDto.GameId = game.Id;
                        activityDto.GameName = game.Data.Name;
                        activityDto.HeaderImage = game.Data.HeaderImage;
                    }
                    if (activityDto.MemberFollowedId != 0)
                    {
                        activityDto.MemberFollowedUsername = await _unitOfWork.userRepository.GetUsernameFromId(activityDto.MemberFollowedId);
                    }
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

            if (!_lobbyTracker.CheckIfMemberInAnyLobby(userId)) return Ok(status);
            var lobbyId = _lobbyTracker.GetLobbyIdFromUser(userId);

            status.InQueue = _lobbyTracker.CheckIfMemberInQueue(lobbyId, userId);
            status.LobbyId = lobbyId;

            return Ok(status);
        }


        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("follow")]
        public async Task<ActionResult> FollowMember(int memberId)
        {
            var userId = GetUserIdFromClaim();
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);
            var userTarget = await _unitOfWork.userRepository.GetUserByIdAsync(memberId);

            if (user == null) return NotFound();

            if (userTarget == null) return NotFound();

            if (!await _unitOfWork.userRepository.CheckIfUserExists(memberId)) return NotFound("Member you are trying to follow doesn't exist");

            if (user.AppUserProfile.AppUserData.Following.Contains(memberId)) return NoContent();

            if (userTarget.AppUserProfile.AppUserData.Followers.Contains(userId)) return NoContent();

            user.AppUserProfile.AppUserData.Following.Add(memberId);
            userTarget.AppUserProfile.AppUserData.Followers.Add(userId);
            _unitOfWork.userRepository.Update(user);
            _unitOfWork.userRepository.Update(userTarget);

            var activityLog = new ActivityLog
            {
                Date = DateTime.Now,
                AppUserId = userId,
                ActivityId = 4,
                MemberFollowedId = memberId
            };

            _unitOfWork.activitiesRepository.AddActivityLog(activityLog);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("unfollow")]
        public async Task<ActionResult> UnFollowMember(int memberId)
        {
            var userId = GetUserIdFromClaim();
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);
            var userTarget = await _unitOfWork.userRepository.GetUserByIdAsync(memberId);

            if (user == null) return NotFound();

            if (userTarget == null) return NotFound();

            if (!await _unitOfWork.userRepository.CheckIfUserExists(memberId)) return NotFound("Member you are trying to follow doesn't exist");

            if (!user.AppUserProfile.AppUserData.Following.Contains(memberId)) return NoContent();

            if (!userTarget.AppUserProfile.AppUserData.Followers.Contains(userId)) return NoContent();

            user.AppUserProfile.AppUserData.Following.Remove(memberId);
            userTarget.AppUserProfile.AppUserData.Followers.Remove(userId);
            _unitOfWork.userRepository.Update(user);
            _unitOfWork.userRepository.Update(userTarget);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("check-follow")]
        public async Task<ActionResult<bool>> CheckIfFollowed(int memberId)
        {
            var userId = GetUserIdFromClaim();
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            if (user.AppUserProfile.AppUserData.Following.Contains(memberId)) return Ok(true);

            return Ok(false);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MemberMeiliDto>>> SearchForMember(string name, int limit = 10)
        {
            var index = _meilisearchService.GetIndex("members");
            var hits = await _meilisearchService.SearchAsync<AppUserMeili>(index, name, new SearchQuery { Limit = limit });
            return Ok(Mapper.Map<IEnumerable<AppUserMeili>>(hits.Hits));
        }

        [HttpGet("check-mail-exists")]
        public async Task<ActionResult<bool>> CheckIfMailExists(string mail)
        {
            return await _unitOfWork.userRepository.CheckIfMailTaken(mail);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPost("set-background")]
        public async Task<ActionResult<MemberPhotoDto>> SetBackground(MemberBackgroundDto background)
        {
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(GetUserIdFromClaim());
            user.AppUserProfile.AccountCustomization.BackgroundUrl = background.Url;
            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Could not set background");
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPost("set-photo")]
        public async Task<ActionResult<MemberPhotoDto>> SetPhoto(IFormFile file)
        {
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new AppUserPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            user.AppUserProfile.AppUserPhoto = photo;

            if (await _unitOfWork.Complete()) return Mapper.Map<MemberPhotoDto>(photo);

            return BadRequest("Problem adding photo");
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}/discord")]
        public async Task<ActionResult<DiscordStatusDto>> DiscordConnection(int id)
        {
            var user = await _unitOfWork.userRepository.GetUserConnectionsFromUid(id);

            if (user == null) return NotFound();

            var dto = new DiscordStatusDto
            {
                DiscordId = user.Discord.DiscordId,
                Connected = user.DiscordConnected,
                Username = user.Discord.Username,
                Discriminator = user.Discord.Discriminator,
                Hidden = user.Discord.Hidden,
                InServer = await _discordBotService.CheckIfUserInServer(user.Discord.DiscordId)
            };
            return dto;
        }
        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}/steam")]
        public async Task<ActionResult<SteamStatusDto>> SteamConnection(int id)
        {
            var user = await _unitOfWork.userRepository.GetUserConnectionsFromUid(id);

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
        public async Task<ActionResult> BlockMember(int id)
        {
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();
            if (user.AppUserProfile.BlockedUsers == null) user.AppUserProfile.BlockedUsers = new List<int>();
            user.AppUserProfile.BlockedUsers.Add(id);
            _unitOfWork.userRepository.Update(user);
            await _unitOfWork.Complete();

            return NoContent();
        }
        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("unblock/{id}")]
        public async Task<ActionResult> UnblockMember(int id)
        {
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();

            user.AppUserProfile.BlockedUsers.Remove(id);
            _unitOfWork.userRepository.Update(user);
            await _unitOfWork.Complete();

            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("discord/unlink")]
        public async Task<ActionResult> UnlinkDiscord()
        {
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();

            user.AppUserProfile.UserConnections.Discord = new DiscordProfile { };
            user.AppUserProfile.UserConnections.DiscordConnected = false;

            _unitOfWork.userRepository.Update(user);
            await _unitOfWork.Complete();

            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("steam/unlink")]
        public async Task<ActionResult> UnlinkSteam()
        {
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();

            user.AppUserProfile.UserConnections.Steam = new SteamProfile { };
            user.AppUserProfile.UserConnections.SteamConnected = false;

            _unitOfWork.userRepository.Update(user);
            await _unitOfWork.Complete();

            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("steam/hide")]
        public async Task<ActionResult> HideSteam(MemberHideConnectionDto memberHideConnectionDto)
        {
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();

            user.AppUserProfile.UserConnections.Steam.Hidden = memberHideConnectionDto.Hide;

            _unitOfWork.userRepository.Update(user);
            await _unitOfWork.Complete();

            return NoContent();
        }
        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("discord/hide")]
        public async Task<ActionResult> HideDiscord(MemberHideConnectionDto memberHideConnectionDto)
        {
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(GetUserIdFromClaim());

            if (user == null) return NotFound();

            user.AppUserProfile.UserConnections.Discord.Hidden = memberHideConnectionDto.Hide;

            _unitOfWork.userRepository.Update(user);
            await _unitOfWork.Complete();

            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("check-blocked")]
        public async Task<ActionResult<bool>> CheckIfBlockedBy(int memberId)
        {
            var userId = GetUserIdFromClaim();
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(memberId);

            if (user == null) return NotFound();

            return user.AppUserProfile.BlockedUsers.Contains(userId);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("check-blocking")]
        public async Task<ActionResult<bool>> CheckIfBlocked(int memberId)
        {
            var userId = GetUserIdFromClaim();
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            return user.AppUserProfile.BlockedUsers.Contains(memberId);
        }
    }
}