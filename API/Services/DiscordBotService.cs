using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces.IServices;
using Discord;
using Discord.WebSocket;

namespace API.Services
{
    public class DiscordBotService : IDiscordBotService
    {
        private DiscordSocketClient _client;
        private Task _ready;
        private readonly IConfiguration _config;
        public DiscordBotService(IConfiguration config)
        {
            _config = config;
            Thread trd = new Thread(new ThreadStart(this.InitClient));
            trd.IsBackground = true;
            trd.Start();
        }

        public async Task<string> CreateVoiceChannelForLobby(ulong[] userIds)
        {
            await _ready;

            // Create the voice channel

            var guild = _client.GetGuild(933804444930412544);

            var channel = await guild.CreateVoiceChannelAsync("Lobby 1", prop => prop.CategoryId = 961239473683853402);

            await channel.SyncPermissionsAsync();

            // Add users to permissions
            foreach (var userId in userIds)
            {
                var user = await _client.GetUserAsync(userId);

                var permissions = new OverwritePermissions(viewChannel: PermValue.Allow, connect: PermValue.Allow, useVoiceActivation: PermValue.Allow);

                await channel.AddPermissionOverwriteAsync(user, permissions);
            }

            // Create invite link for users to join
            var inviteLink = await channel.CreateInviteAsync();
            return inviteLink.Url;
        }
        private async void InitClient()
        {
            _client = new DiscordSocketClient();
            _client.Log += Logger.Log;
            _client.Ready += ReadyAsync;
            // Login and connect.

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var token = "";
            if (env == "Development")
            {
                token = _config.GetSection("DISCORD_APP")["DISCORD_CLIENT_TOKEN"];
            }
            else
            {
                token = Environment.GetEnvironmentVariable("DISCORD_CLIENT_TOKEN");
            }

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            _ready = Task.CompletedTask;

            return Task.CompletedTask;
        }

    }
}