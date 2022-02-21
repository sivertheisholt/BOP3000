using API.DTOs.GameApps;
using API.DTOs.SteamApps;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using AutoMapper;
using Meilisearch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AppsController : BaseApiController
    {
        private readonly IMeilisearchService _meilisearchService;
        private readonly ISteamAppRepository _steamAppRepository;
        public AppsController(IMapper mapper, IMeilisearchService meilisearchService, ISteamAppRepository steamAppRepository) : base(mapper)
        {
            _steamAppRepository = steamAppRepository;
            _meilisearchService = meilisearchService;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AppListInfoDto>>> SearchForApp(string name, int limit = 10)
        {
            var hits = await _meilisearchService.SearchForAppAsync(name, new SearchQuery { Limit = limit });
            return Ok(Mapper.Map<IEnumerable<AppListInfoDto>>(hits.Hits));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<GameAppInfoDto>>> GetActiveApps()
        {
            var apps = await _steamAppRepository.GetActiveApps();
            return Mapper.Map<List<GameAppInfoDto>>(apps);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GameAppInfoDto>> GetGameAppInfo(int id)
        {
            var app = await _steamAppRepository.GetAppInfoAsync(id);
            Console.WriteLine(app.Data);

            if (app == null) return NotFound();

            return Ok(Mapper.Map<GameAppInfoDto>(app.Data));
        }
    }
}