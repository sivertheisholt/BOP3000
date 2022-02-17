using System.ComponentModel.DataAnnotations;
using API.Entities.Users;

namespace API.Entities.Applications
{
    public class Discord
    {
        public AppUserConnections AppUserConnections { get; set; }
        public int AppUserConnectionsId { get; set; }
    }
}