using API.DTOs;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MembersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public MembersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMember(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) return NotFound();

            return Ok(_mapper.Map<MemberDto>(user));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
        {
            var users = await _userRepository.GetUsersAsync();

            if (users == null) return NotFound();

            return Ok(_mapper.Map<IEnumerable<MemberDto>>(users));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPut("")]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var userId = GetUserIdFromClaim();

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest();
        }
    }
}