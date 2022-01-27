using API.Entities.Applications;

namespace API.Entities.Users
{
    public class AppUserConnections
    {
        public int Id { get; set; }
        public Steam Steam { get; set; }
        public Discord Discord { get; set; }
    }
}