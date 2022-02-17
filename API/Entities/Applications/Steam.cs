using System.ComponentModel.DataAnnotations;
using API.Entities.Users;

namespace API.Entities.Applications
{
    public class Steam
    {
        public AppUserConnections AppUserConnections { get; set; }
        public int AppUserConnectionsId { get; set; }
    }
}