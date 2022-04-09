using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Activities;

namespace API.Interfaces.IRepositories
{
    public interface IActivityRepository : IBaseRepository<Activity>
    {
        Task<Activity> GetActivity(int id);

        void AddActivity(Activity activity);

        Task<List<Activity>> GetActivities();

    }
}