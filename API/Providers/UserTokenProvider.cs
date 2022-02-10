using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace API.Providers
{
    public class UserTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : AppUser
    {
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            if (manager != null && user != null)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public string GenerateToken(AppUser user, string purpose)
        {
            string secretString = "coffeIsGood";
            return secretString + user.Email + purpose + user.Id;
        }

        public Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(GenerateToken(user, purpose));
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(token == GenerateToken(user, purpose));
        }
    }
}