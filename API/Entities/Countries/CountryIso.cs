using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Countries
{
    public class CountryIso
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TwoLetterCode { get; set; }
        public string ThreeLetterCode { get; set; }
        public string NumericCode { get; set; }
    }
}