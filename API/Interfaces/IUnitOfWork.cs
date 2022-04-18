using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.IRepositories;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        IActivitiesRepository activitiesRepository { get; }
        IActivityRepository activityRepository { get; }
        ICountryRepository countryRepository { get; }
        ILobbiesRepository lobbiesRepository { get; }
        ISteamAppsRepository steamAppsRepository { get; }
        ISteamAppRepository steamAppRepository { get; }

        Task<bool> Complete();
        bool HasChanged();
    }
}