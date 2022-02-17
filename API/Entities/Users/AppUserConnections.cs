using API.Entities.Applications;

namespace API.Entities.Users
{
    public class AppUserConnections
    {
        public bool SteamConnected { get; set; }
        public bool DiscordConnected { get; set; }
        public Steam Steam { get; set; }
        public Discord Discord { get; set; }

        public AppUserProfile AppUserProfile { get; set; }
        public int AppUserProfileId { get; set; }
    }
}