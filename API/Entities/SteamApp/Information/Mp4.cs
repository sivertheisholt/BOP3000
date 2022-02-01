using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class Mp4
    {
        public int Id { get; set; }
        public string _480 { get; set; }
        public string Max { get; set; }
        public Movy Movy { get; set; }
        public int MovyId { get; set; }
    }
}