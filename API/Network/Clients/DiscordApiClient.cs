using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Applications;
using API.Entities.Applications;
using API.Interfaces.IClients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Network.Clients
{
    public class DiscordApiClient : BaseClient, IDiscordApiClient
    {
        private readonly IConfiguration _configuration;
        public DiscordApiClient(HttpClient client, IConfiguration configuration) : base(client)
        {
            var uri = "https://discord.com/api";

            _configuration = configuration;

            Client.BaseAddress = new Uri(uri);
        }

        public async Task<DiscordUserDto> GetUserObjectFromToken(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Client.BaseAddress}/users/@me");
            request.Headers.Add("Authorization", "Bearer " + token);
            using (var response = await Client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Something wrong happen");
                }

                // Read Response Content (this will usually be JSON content)
                var content = await response.Content.ReadAsStringAsync();

                JObject contentObject = JObject.Parse(content);

                var resultString = JsonConvert.SerializeObject(contentObject);

                // Deserialize the JSON string
                var userResult = JsonConvert.DeserializeObject<DiscordUserDto>(resultString);

                return userResult;
            }
        }

        public async Task<DiscordTokenDto> RefreshToken(string refreshToken)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var clientId = "";
            var clientSecret = "";
            if (env == "Development")
            {
                clientId = _configuration.GetSection("DISCORD_APP")["DISCORD_CLIENT_ID"];
                clientSecret = _configuration.GetSection("DISCORD_APP")["DISCORD_CLIENT_SECRET"];
            }
            else
            {
                clientId = Environment.GetEnvironmentVariable("DISCORD_CLIENT_ID");
                clientSecret = Environment.GetEnvironmentVariable("DISCORD_CLIENT_SECRET");
            }
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://discord.com/api/oauth2/token?client_id={clientId}&client_secret={clientSecret}&grant_type=refresh_token&refresh_token={refreshToken}");
            using (var response = await Client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Something wrong happen");
                }
                // Read Response Content (this will usually be JSON content)
                var content = await response.Content.ReadAsStringAsync();

                JObject contentObject = JObject.Parse(content);

                var resultString = JsonConvert.SerializeObject(contentObject);

                // Deserialize the JSON string
                var tokenResult = JsonConvert.DeserializeObject<DiscordTokenDto>(resultString);

                return tokenResult;
            }
        }
    }
}