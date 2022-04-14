using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities.Users;
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
        private readonly IMeilisearchService _meilisearchService;
        private readonly IUserRepository _userRepository;
        public TestController(IMapper mapper, ISteamAppRepository steamAppRepository, ISteamAppsRepository steamAppsRepository, ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient, ILobbiesRepository lobbiesRepository, IMeilisearchService meilisearchService, IUserRepository userRepository) : base(mapper)
        {
            _userRepository = userRepository;
            _meilisearchService = meilisearchService;
            _lobbiesRepository = lobbiesRepository;
            _steamAppsClient = steamAppsClient;
            _steamStoreClient = steamStoreClient;
            _steamAppsRepository = steamAppsRepository;
            _steamAppRepository = steamAppRepository;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("seed_user_index")]
        public async Task<ActionResult> SeedUsersMeili()
        {
            var createTask = _meilisearchService.CreateIndexAsync("members");

            var users = Mapper.Map<List<AppUserMeili>>(await _userRepository.GetUsersMeiliAsync());

            var cont = createTask.ContinueWith(task =>
            {
                var index = _meilisearchService.GetIndex("members");

                var docsTask = _meilisearchService.AddDocumentsAsync(users.ToArray(), index);

                var docs = docsTask.ContinueWith(docsTask =>
                {
                    Console.WriteLine("Meilisearch docs successfully uploaded");
                });
                docs.Wait();
            });

            cont.Wait();
            return Ok();
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