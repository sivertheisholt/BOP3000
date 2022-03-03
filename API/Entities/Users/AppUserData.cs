using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Users
{
    public class AppUserData
    {
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public ICollection<int> Followers { get; set; }
        public ICollection<int> Following { get; set; }
        public ICollection<int> UserFavoriteGames { get; set; }
        public AppUserProfile AppUserProfile { get; set; }
        public int AppUserProfileId { get; set; }
    }
}