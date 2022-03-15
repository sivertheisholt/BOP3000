using API.DTOs.Members;
using API.Interfaces.IRepositories;
using API.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MembersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly LobbyTracker _lobbyTracker;
        public MembersController(IUserRepository userRepository, IMapper mapper, ICountryRepository countryRepository, LobbyTracker lobbyTracker) : base(mapper)
        {
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
    }
}