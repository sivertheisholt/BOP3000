using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Countries;

namespace API.DTOs.Members
{
    public class MemberProfileDto
    {

        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public CountryIsoDto CountryIso { get; set; }
        public MemberDataDto MemberData { get; set; }
        public MemberPhotoDto MemberPhoto { get; set; }
        public MemberCustomizationDto MemberCustomization { get; set; }
    }
}