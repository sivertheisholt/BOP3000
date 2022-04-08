using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SteamApps.Interfaces.IRepositories;

namespace SteamApps.Data.Repositories
{
    abstract public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DataContext _context;
        protected BaseRepository(DataContext context)
        {
            _context = context;
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        protected DataContext Context { get { return _context; } }
    }
}