using API.DTOs.Members;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MembersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        public MembersController(IUserRepository userRepository, IMapper mapper, ICountryRepository countryRepository) : base(mapper)
        {
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
        [HttpPatch("")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
        {
            var users = await _userRepository.GetUsersAsync();

            if (users == null) return NotFound();

            return Ok(Mapper.Map<IEnumerable<MemberDto>>(users));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPut("")]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var userId = GetUserIdFromClaim();

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            var member = Mapper.Map(memberUpdateDto, user);
            member.AppUserProfile.CountryIso = await _countryRepository.GetCountryIsoByIdAsync(memberUpdateDto.CountryId);
            member.AppUserProfile.Birthday = DateTime.Parse(memberUpdateDto.Birthday);
            member.AppUserProfile.Gender = memberUpdateDto.Gender;

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
    }
}