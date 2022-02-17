using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Members
{
    public class MemberDataDto
    {
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string UserDescription { get; set; }
        public ICollection<int> Followers { get; set; }
        public ICollection<int> Following { get; set; }
        public ICollection<int> UserFavoriteGames { get; set; }
    }
}