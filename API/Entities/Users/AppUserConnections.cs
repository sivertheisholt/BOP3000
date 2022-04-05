using API.Entities.Applications;

namespace API.Entities.Users
{
    public class AppUserConnections
    {
        public bool SteamConnected { get; set; }
        public bool DiscordConnected { get; set; }
        public SteamProfile Steam { get; set; }
        public DiscordProfile Discord { get; set; }

        public AppUserProfile AppUserProfile { get; set; }
        public int AppUserProfileId { get; set; }
    }
}