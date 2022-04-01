using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Activities;
using API.Interfaces.IRepositories;

namespace API.Data.Repositories
{
    public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(DataContext context) : base(context)
        {
        }

        public Task AddActivity(Activity activity)
        {
            Context.Activity.Add(activity);
            return Task.CompletedTask;
        }

        public async Task<Activity> GetActivity(int id)
        {
            return await Context.Activity.FindAsync(id);
        }
    }
}