using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces.IRepositories
{
    public interface IBaseRepository<in T> where T : class
    {
        void Update(T entity);

        void Delete(T entity);

        Task resetId(string tableName);
    }
}