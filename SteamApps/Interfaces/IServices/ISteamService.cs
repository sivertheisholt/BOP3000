using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SteamApps.Interfaces.IServices
{
    public interface ISteamService : IHostedService
    {
        Task InitDatabase();
    }
}