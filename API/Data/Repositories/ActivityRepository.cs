using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Activities;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(DataContext context) : base(context)
        {
        }

        public void AddActivity(Activity activity)
        {
            Context.Activity.Add(activity);
        }

        public async Task<List<Activity>> GetActivities()
        {
            return await Context.Activity.ToListAsync();
        }

        public async Task<Activity> GetActivity(int id)
        {
            return await Context.Activity.FindAsync(id);
        }
    }
}