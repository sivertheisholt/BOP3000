using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> SaveAllAsync();
        void Update(T entity);

        void Delete(T entity);
    }
}