using API.DTOs;
using API.DTOs.Accounts;
using API.Entities.Applications;
using API.Entities.Users;
using API.Enums;
using API.Extentions;
using API.Interfaces;
using API.Interfaces.IClients;
using API.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        /// <summary>
        /// Constructs a new AccountController
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="tokenService"></param>
        private readonly IDiscordApiClient _discordApiClient;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        public AccountsController(IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IDiscordApiClient discordApiClient, IEmailService emailService, IUnitOfWork unitOfWork) : base(mapper)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _discordApiClient = discordApiClient;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
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
                    CountryIso = await _unitOfWork.countryRepository.GetCountryIsoByIdAsync(registerDto.CountryId),
                    Gender = registerDto.Gender,
                    AppUserData = new AppUserData
                    {
                        Followers = new List<int>(),
                        Following = new List<int>(),
                        FinishedLobbies = new List<int>(),
                        UserFavoriteGames = new List<int>(),
                    },
                    AppUserPhoto = new AppUserPhoto
                    {
                        Url = "https://res.cloudinary.com/dzpzecnx5/image/upload/v1649157462/933-9332131_profile-picture-default-png_i8rgef.png"
                    },
                    AccountCustomization = new AppUserCustomization
                    {
                        BackgroundUrl = "http://res.cloudinary.com/dzpzecnx5/image/upload/v1650544575/AccountBackgrounds/pexels-suzukii-xingfu-698319_brqyfl.jpg"
                    },
                    UserConnections = new AppUserConnections
                    {
                        Discord = new DiscordProfile { },
                        Steam = new SteamProfile { },
                        DiscordConnected = false,
                        SteamConnected = false,
                    },
                    BlockedUsers = new List<int>()
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
            var user = await _unitOfWork.userRepository.GetUserByEmailAsync(loginDto.Email);

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

        [HttpGet("steam")]
        public async Task<IActionResult> SignInSteam()
        {
            var provider = "Steam";
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            if (!await HttpContext.IsProviderSupportedAsync(provider))
            {
                return BadRequest();
            }

            return Challenge(new AuthenticationProperties { RedirectUri = "/settings?success=true&provider=steam" }, provider);
        }
        [HttpGet("discord")]
        public async Task<IActionResult> SignInDiscord()
        {
            var provider = "Discord";
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            if (!await HttpContext.IsProviderSupportedAsync(provider))
            {
                return BadRequest();
            }

            return Challenge(new AuthenticationProperties { RedirectUri = "/settings?success=true&provider=discord" }, provider);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("steam-success")]
        public async Task<IActionResult> SaveSteamId()
        {
            var steamId = HttpContext.Request.Cookies["steamId"];

            if (steamId == null) return NotFound();

            var uid = GetUserIdFromClaim();

            var user = await _unitOfWork.userRepository.GetUserByIdAsync(uid);

            if (user == null) return NotFound();
            if (user.AppUserProfile.UserConnections.SteamConnected) return BadRequest("Steam account already connected");
            if (await _unitOfWork.userRepository.CheckIfSteamAccountExists(Int64.Parse(steamId))) return BadRequest("Steam account already connected to another user");

            _unitOfWork.userRepository.AddSteamId(user, Int64.Parse(steamId));
            await _unitOfWork.Complete();

            return Ok();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPatch("discord-success")]
        public async Task<IActionResult> SaveDiscordToken()
        {
            var access_token = HttpContext.Request.Cookies["access_token"];
            var refresh_token = HttpContext.Request.Cookies["refresh_token"];
            var token_expires = HttpContext.Request.Cookies["token_expires"];

            if (access_token == null) return NotFound();

            var uid = GetUserIdFromClaim();

            var user = await _unitOfWork.userRepository.GetUserByIdAsync(uid);

            if (user == null) return NotFound();

            if (user.AppUserProfile.UserConnections.DiscordConnected) return BadRequest("Discord account already connected");

            var userObject = await _discordApiClient.GetUserObjectFromToken(access_token);

            if (await _unitOfWork.userRepository.CheckIfDiscordAccountExists(ulong.Parse(userObject.Id))) return BadRequest("Discord account already connected to another user");

            var discord = new DiscordProfile
            {
                DiscordId = ulong.Parse(userObject.Id),
                Username = userObject.Username,
                Discriminator = userObject.Discriminator,
                RefreshToken = refresh_token,
                AccessToken = access_token,
                Expires = DateTime.Parse(token_expires)
            };

            _unitOfWork.userRepository.AddDiscord(user, discord);
            await _unitOfWork.Complete();

            return Ok();
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
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);

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
            var user = await _unitOfWork.userRepository.GetUserByEmailAsync(forgottenPasswordDto.Email);

            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            _emailService.SendForgottenPasswordMail(token, forgottenPasswordDto.Email);

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
            var user = await _unitOfWork.userRepository.GetUserByEmailAsync(changeForgottenPasswordDto.Email);

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
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            _unitOfWork.Complete();
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