using API.DTOs;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// UsersController contains all the endpoints for actions related to users
    /// </summary>
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new UsersController
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>A collection of MemberDto</returns>
        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();

            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            return Ok(usersToReturn);
        }

        /// <summary>
        /// Gets a user by ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A MemberDto</returns>
        //[Authorize(Roles = "Member")]
        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            return _mapper.Map<MemberDto>(user);
        }
    }
}