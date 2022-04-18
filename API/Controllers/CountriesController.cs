using API.DTOs.Countries;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CountriesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public CountriesController(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryIsoDto>> GetCountry(int id)
        {
            var country = await _unitOfWork.countryRepository.GetCountryIsoByIdAsync(id);

            if (country == null) return NotFound();

            return Ok(Mapper.Map<CountryIsoDto>(country));
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CountryIsoDto>>> GetCountries()
        {
            var country = await _unitOfWork.countryRepository.GetAllCountriesAsync();

            if (country == null) return NotFound();

            return Ok(Mapper.Map<IEnumerable<CountryIsoDto>>(country));
        }
    }
}
