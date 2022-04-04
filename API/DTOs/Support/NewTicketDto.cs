using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Support
{
    public class NewTicketDto
    {
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}