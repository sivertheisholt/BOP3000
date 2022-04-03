using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Users
{
    public class AppUserPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public AppUserProfile AppUserProfile { get; set; }
        public int AppUserProfileId { get; set; }
    }
}