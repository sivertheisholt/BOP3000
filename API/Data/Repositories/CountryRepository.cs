using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Countries;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class CountryRepository : BaseRepository<CountryIso>, ICountryRepository
    {
        public CountryRepository(DataContext context) : base(context)
        {
        }

        public void AddCountryIso(CountryIso countryIso)
        {
            Context.CountryIso.Add(countryIso);
        }

        public async Task<ICollection<CountryIso>> GetAllCountriesAsync()
        {
            return await Context.CountryIso.ToListAsync();
        }

        public async Task<CountryIso> GetCountryIsoByIdAsync(int id)
        {
            return await Context.CountryIso.FindAsync(id);
        }
    }
}