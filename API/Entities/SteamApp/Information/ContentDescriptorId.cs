using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.SteamApp.Information
{
    public class ContentDescriptorId
    {
        public int Id { get; set; }
        public ContentDescriptors ContentDescriptors { get; set; }
        public int ContentDescriptorsId { get; set; }
    }
}