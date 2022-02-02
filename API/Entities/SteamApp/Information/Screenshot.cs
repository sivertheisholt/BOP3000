using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class Screenshot
    {
        public int Id { get; set; }
        public string Path_thumbnail { get; set; }
        public string Path_full { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}