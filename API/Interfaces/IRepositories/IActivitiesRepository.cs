using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Activities;

namespace API.Interfaces.IRepositories
{
    public interface IActivitiesRepository : IBaseRepository<ActivityLog>
    {
        Task<List<ActivityLog>> GetActivitiesForUser(int uid);

        void AddActivityLog(ActivityLog activityLog);

        Task<List<ActivityLog>> GetActivities();

    }
}