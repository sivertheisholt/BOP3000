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

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryIsoDto>> GetCountry(int id)
        {
            var country = await _countryRepository.GetCountryIsoByIdAsync(id);

            if (country == null) return NotFound();

            return Ok(Mapper.Map<CountryIsoDto>(country));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CountryIsoDto>>> GetCountries()
        {
            var country = await _countryRepository.GetAllCountriesAsync();

            if (country == null) return NotFound();

            return Ok(Mapper.Map<IEnumerable<CountryIsoDto>>(country));
        }
    }
}