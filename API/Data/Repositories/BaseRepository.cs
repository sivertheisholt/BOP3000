using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
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
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task resetId(string tableName)
        {
            await _context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('{tableName}', RESEED, 0)");
        }

        protected DataContext Context { get { return _context; } }
    }
}