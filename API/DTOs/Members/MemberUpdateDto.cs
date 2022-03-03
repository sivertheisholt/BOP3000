using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Countries;

namespace API.DTOs.Members
{
    public class MemberUpdateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Description { get; set; }
    }
}