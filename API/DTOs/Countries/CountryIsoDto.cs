using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Countries
{
    public class CountryIsoDto
    {
        public string Name { get; set; }
        public string TwoLetterCode { get; set; }
        public string ThreeLetterCode { get; set; }
        public string NumericCode { get; set; }
    }
}