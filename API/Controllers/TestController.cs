using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
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
        private readonly IEmailService _emailservice;
        private readonly IMeilisearchService _meilisearchService;
        public TestController(IMapper mapper, ISteamAppRepository steamAppRepository, ISteamAppsRepository steamAppsRepository, ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient, ILobbiesRepository lobbiesRepository, IMeilisearchService meilisearchService) : base(mapper)
        {
            _meilisearchService = meilisearchService;
            //_emailservice = emailservice;
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

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("send_email")]
        public async Task<ActionResult> SendEmail()
        {
            await Task.Run(() => _emailservice.SendForgottenPasswordMail("test", "playfu3000@gmail.com"));
            return NoContent();
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("seed_index")]
        public async Task<ActionResult> SeedIndex()
        {
            var apps = await _steamAppsRepository.GetAppsList(1);

            var appsArray = apps.Apps.ToArray();

            var createTask = _meilisearchService.CreateIndexAsync("apps");

            var cont = createTask.ContinueWith(task =>
            {
                var index = _meilisearchService.GetIndex("apps");

                var docsTask = _meilisearchService.AddDocumentsAsync(appsArray, index);

                var docs = docsTask.ContinueWith(docsTask =>
                {
                    Console.WriteLine("Docs successfully uploaded");
                });
                docs.Wait();
            });

            cont.Wait();
            return NoContent();
        }
    }
}