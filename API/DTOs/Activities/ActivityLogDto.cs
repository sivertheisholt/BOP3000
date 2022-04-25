using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Activities
{
    public class ActivityLogDto
    {
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public int AppUserId { get; set; }
        public string Identifier { get; set; }
        public int LobbyId { get; set; }
        public string GameName { get; set; }
        public int GameId { get; set; }
        public string HeaderImage { get; set; }
        public string ProfilePicture { get; set; }
    }
}