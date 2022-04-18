using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Lobbies
{
    public class LogDto
    {
        public ICollection<MessageDto> Messages { get; set; }
    }
}