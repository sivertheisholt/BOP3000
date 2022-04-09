using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Activities;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class ActivitiesRepository : BaseRepository<ActivityLog>, IActivitiesRepository
    {
        public ActivitiesRepository(DataContext context) : base(context)
        {

        }
        public void AddActivityLog(ActivityLog activityLog)
        {
            Context.Add(activityLog);
        }

        public async Task<List<ActivityLog>> GetActivities()
        {
            return await Context.ActivityLog.ToListAsync();
        }

        public Task<List<ActivityLog>> GetActivitiesForUser(int uid)
        {
            return Task.FromResult(Context.ActivityLog.Where(x => x.AppUserId == uid).Include(x => x.Activity).OrderByDescending(x => x.Id).ToList());
        }
    }
}