using System.Text;
using API.Data;
using API.Entities.Roles;
using API.Entities.Users;
using API.Enums;
using API.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extentions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentityCore<AppUser>(opt =>
            {

            }).AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddRoleValidator<RoleValidator<AppRole>>()
                .AddEntityFrameworkStores<DataContext>()
                .AddTokenProvider("Default", typeof(UserTokenProvider<AppUser>));

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var token = "";
            if (env == "Development")
            {
                token = config["TokenKey"];
            }
            else
            {
                token = Environment.GetEnvironmentVariable("JWT_TOKEN_KEY");
            }
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddSteam()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequireMemberRole", policy => policy.RequireRole(Role.Member.MakeString()));
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole(Role.Admin.MakeString()));
                opt.AddPolicy("RequirePremiumRole", policy => policy.RequireRole(Role.Premium.MakeString()));
            });

            return services;
        }
    }
}