using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Countries;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CountriesController : BaseApiController
    {
        private readonly ICountryRepository _countryRepository;
        public CountriesController(IMapper mapper, ICountryRepository countryRepository) : base(mapper)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryIsoDto>> GetCountry(int id)
        {
            var country = await _countryRepository.GetCountryIsoByIdAsync(id);

            if (country == null) return NotFound();

            return Ok(Mapper.Map<CountryIsoDto>(country));
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CountryIsoDto>>> GetCountries()
        {
            var country = await _countryRepository.GetAllCountriesAsync();

            if (country == null) return NotFound();

            return Ok(Mapper.Map<IEnumerable<CountryIsoDto>>(country));
        }
    }
}
