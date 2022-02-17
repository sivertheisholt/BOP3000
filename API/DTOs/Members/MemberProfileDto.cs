using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Countries;

namespace API.DTOs.Members
{
    public class MemberProfileDto
    {
        public int Age { get; set; }
        public string Gender { get; set; }
        public CountryIsoDto CountryIso { get; set; }
        public MemberDataDto MemberData { get; set; }
    }
}