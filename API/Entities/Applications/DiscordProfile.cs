using System.ComponentModel.DataAnnotations;
using API.Entities.Users;

namespace API.Entities.Applications
{
    public class DiscordProfile
    {
        public ulong DiscordId { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
        public AppUserConnections AppUserConnections { get; set; }
        public int AppUserConnectionsId { get; set; }
    }
}