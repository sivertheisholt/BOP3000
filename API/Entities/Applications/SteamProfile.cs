using System.ComponentModel.DataAnnotations;
using API.Entities.Users;

namespace API.Entities.Applications
{
    public class SteamProfile
    {
        public long SteamId { get; set; }
        public bool Hidden { get; set; }
        public AppUserConnections AppUserConnections { get; set; }
        public int AppUserConnectionsId { get; set; }
    }
}