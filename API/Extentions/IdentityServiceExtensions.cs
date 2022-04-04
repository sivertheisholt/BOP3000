using System.Security.Claims;
using System.Text;
using API.Data;
using API.Entities.Roles;
using API.Entities.Users;
using API.Enums;
using API.Providers;
using AspNet.Security.OpenId;
using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/signout";
                })
                .AddSteam(options =>
                {
                    var appId = "";
                    if (env == "Development")
                    {
                        appId = config["STEAM_APP_ID"];
                    }
                    else
                    {
                        appId = Environment.GetEnvironmentVariable("STEAM_APP_ID");
                    }
                    options.ApplicationKey = appId;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Events.OnAuthenticated = ctx =>
                    {
                        // Get the last segment which is the steam steamid
                        var steamId = new Uri(ctx.Identifier).Segments.Last();

                        //Temp just to get it work, this is not safe!!
                        ctx.Response.Cookies.Append("steamId", steamId);

                        return Task.CompletedTask;
                    };
                })
                .AddDiscord(options =>
                {
                    var clientId = "";
                    var clientSecret = "";
                    if (env == "Development")
                    {
                        clientId = config.GetSection("DiscordApp")["ClientId"];
                        clientSecret = config.GetSection("DiscordApp")["ClientSecret"];
                    }
                    else
                    {
                        clientId = Environment.GetEnvironmentVariable("DISCORD_APP_CLIENT_ID");
                        clientSecret = Environment.GetEnvironmentVariable("DISCORD_APP_CLIENT_SECRET");
                    }
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Events.OnTicketReceived = ctx =>
                    {
                        var access_token = ctx.Properties.Items[".Token.access_token"];
                        var refresh_token = ctx.Properties.Items[".Token.refresh_token"];
                        var expires = ctx.Properties.Items[".Token.expires_at"];

                        //Temp just to get it work, this is not safe!!
                        ctx.Response.Cookies.Append("access_token", access_token);
                        ctx.Response.Cookies.Append("refresh_token", refresh_token);
                        ctx.Response.Cookies.Append("token_expires", expires);

                        return Task.CompletedTask;
                    };
                    options.SaveTokens = true;
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