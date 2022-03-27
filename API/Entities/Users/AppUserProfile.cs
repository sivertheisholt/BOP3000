using API.Entities.Countries;
using API.Entities.Lobbies;

namespace API.Entities.Users
{
    public class AppUserProfile
    {
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public AppUserConnections UserConnections { get; set; }
        public CountryIso CountryIso { get; set; }
        public AppUserData AppUserData { get; set; }

        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}