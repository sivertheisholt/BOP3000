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
    }
}