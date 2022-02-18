using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TestController : BaseApiController
    {
        private readonly ISteamAppRepository _steamAppRepository;
        private readonly ISteamAppsRepository _steamAppsRepository;
        private readonly ISteamStoreClient _steamStoreClient;
        private readonly ISteamAppsClient _steamAppsClient;
        private readonly ILobbiesRepository _lobbiesRepository;
        public TestController(IMapper mapper, ISteamAppRepository steamAppRepository, ISteamAppsRepository steamAppsRepository, ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient, ILobbiesRepository lobbiesRepository) : base(mapper)
        {
            _lobbiesRepository = lobbiesRepository;
            _steamAppsClient = steamAppsClient;
            _steamStoreClient = steamStoreClient;
            _steamAppsRepository = steamAppsRepository;
            _steamAppRepository = steamAppRepository;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("seed_steam_app_info")]
        public async Task<ActionResult> SeedSteamAppInfo()
        {
            await Seed.SeedSteamAppsInfo(_steamAppRepository, _steamAppsRepository, _steamStoreClient, _steamAppsClient);
            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("seed_lobbies")]
        public async Task<ActionResult> SeedLobbies()
        {
            await Seed.SeedLobbies(_lobbiesRepository);
            return NoContent();
        }
    }
}