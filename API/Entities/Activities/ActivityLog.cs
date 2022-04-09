using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Users;

namespace API.Entities.Activities
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int LobbyId { get; set; }
        public int MemberFollowedId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public Activity Activity { get; set; }
        public int ActivityId { get; set; }
    }
}