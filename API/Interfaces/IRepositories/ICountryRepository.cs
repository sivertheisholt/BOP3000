using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Countries;

namespace API.Interfaces.IRepositories
{
    public interface ICountryRepository : IBaseRepository<CountryIso>
    {
        void AddCountryIso(CountryIso countryIso);

        Task<CountryIso> GetCountryIsoByIdAsync(int id);

    }
}