using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Repositories;
using API.Interfaces;
using API.Interfaces.IRepositories;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IUserRepository userRepository => new UserRepository(_context);

        public IActivitiesRepository activitiesRepository => new ActivitiesRepository(_context);

        public IActivityRepository activityRepository => new ActivityRepository(_context);

        public ICountryRepository countryRepository => new CountryRepository(_context);

        public ILobbiesRepository lobbiesRepository => new LobbiesRepository(_context);

        public ISteamAppsRepository steamAppsRepository => new SteamAppsRepository(_context);

        public ISteamAppRepository steamAppRepository => new SteamAppRepository(_context);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanged()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}