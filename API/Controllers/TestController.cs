using API.Entities.Users;
using API.Interfaces;
using API.Interfaces.IClients;
using API.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TestController : BaseApiController
    {
        private readonly ISteamStoreClient _steamStoreClient;
        private readonly ISteamAppsClient _steamAppsClient;
        private readonly IMeilisearchService _meilisearchService;
        private readonly IUnitOfWork _unitOfWork;
        public TestController(IMapper mapper, ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient, IMeilisearchService meilisearchService, IUnitOfWork unitOfWork) : base(mapper)
        {
            _unitOfWork = unitOfWork;
            _meilisearchService = meilisearchService;
            _steamAppsClient = steamAppsClient;
            _steamStoreClient = steamStoreClient;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("seed_user_index")]
        public async Task<ActionResult> SeedUsersMeili()
        {
            var createTask = _meilisearchService.CreateIndexAsync("members");

            var users = Mapper.Map<List<AppUserMeili>>(await _unitOfWork.userRepository.GetUsersMeiliAsync());

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
            var apps = await _unitOfWork.steamAppsRepository.GetAppsList(1);

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