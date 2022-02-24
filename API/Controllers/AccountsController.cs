using API.DTOs;
using API.DTOs.Accounts;
using API.Entities.Users;
using API.Enums;

using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// AccountController is the endpoint for actions related to accounts
    /// </summary>
    public class AccountsController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        /// <summary>
        /// Constructs a new AccountController
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="tokenService"></param>
        public AccountsController(IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IUserRepository userRepository) : base(mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }


        /// <summary>
        /// Registers a new user with the provided information
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns>A UserDto of the newly created user</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            // Check if username already exists
            if (await UsernameExists(registerDto.Username)) return BadRequest("Username is taken");

            // Check if Email already exists
            if (await EmailExists(registerDto.Email)) return BadRequest("Email is taken");

            // Creates a new AppUser
            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                Email = registerDto.Email.ToLower(),
                AppUserProfile = new AppUserProfile
                {
                    AppUserData = new AppUserData
                    {

                    }
                }
            };

            // Creates the user in backend
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            // Give Member role to user
            var roleResult = await _userManager.AddToRoleAsync(user, Role.Member.MakeString());

            // Check if role was successfully given
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            // Returns a new UserDto
            return new UserDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        /// <summary>
        /// Log in a user with the provided information
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>A UserDto of the newly logged in user</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Gets the user by email from the database
            //var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

            // Checks if user exists
            if (user == null) return Unauthorized("Invalid email");

            // Checks if the password matches
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();

            // Returns a new UserDto
            return new UserDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        /// <summary>
        /// Deletes an account from the database
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "RequireMemberRole")]
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete()
        {
            var userId = GetUserIdFromClaim();
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest("Could not delete user");

            return Ok("User deleted successfully!");
        }

        /// <summary>
        /// Will generate a token for the user to use for resetting password
        /// </summary>
        /// <param name="forgottenPasswordDto"></param>
        /// <returns></returns>
        [HttpPost("forgotten_password")]
        public async Task<ActionResult> ForgottenPassword(ForgottenPasswordDto forgottenPasswordDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(forgottenPasswordDto.Email);

            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Accepted();
        }

        /// <summary>
        /// Verifies the provided token and changes the users password to the provided password 
        /// </summary>
        /// <param name="changeForgottenPasswordDto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPatch("change_forgotten_password")]
        public async Task<ActionResult> ChangeForgottenPassword(ChangeForgottenPasswordDto changeForgottenPasswordDto, string token)
        {
            //Get user
            var user = await _userRepository.GetUserByEmailAsync(changeForgottenPasswordDto.Email);

            if (user == null) return NotFound();

            var result = await _userManager.ResetPasswordAsync(user, token, changeForgottenPasswordDto.NewPassword);

            //Verify token
            if (!result.Succeeded) return Unauthorized();

            return NoContent();
        }

        /// <summary>
        /// Will change the password of the user
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("change_password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userId = GetUserIdFromClaim();
            var user = await _userRepository.GetUserByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded) BadRequest(result.Errors);

            Console.WriteLine(result.Succeeded);

            return NoContent();
        }

        /// <summary>
        /// Checks if username exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Task -> True/False if the username exists</returns>
        private async Task<bool> UsernameExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        /// <summary>
        /// Checks if email exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Task -> True/False if the email exists</returns>
        private async Task<bool> EmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

    }
}