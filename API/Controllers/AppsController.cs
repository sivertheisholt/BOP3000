using API.DTOs.GameApps;
using API.DTOs.SteamApps;
using API.Entities.SteamApps;
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
        private readonly ILobbiesRepository _lobbiesRepository;
        public AppsController(IMapper mapper, IMeilisearchService meilisearchService, ISteamAppRepository steamAppRepository, ILobbiesRepository lobbiesRepository) : base(mapper)
        {
            _lobbiesRepository = lobbiesRepository;
            _steamAppRepository = steamAppRepository;
            _meilisearchService = meilisearchService;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AppListInfoDto>>> SearchForApp(string name, int limit = 10)
        {
            var index = _meilisearchService.GetIndex("apps");
            var hits = await _meilisearchService.SearchAsync<AppListInfo>(index, name, new SearchQuery { Limit = limit });
            return Ok(Mapper.Map<IEnumerable<AppListInfoDto>>(hits.Hits));
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<GameAppInfoDto>>> GetActiveApps()
        {
            var apps = await _steamAppRepository.GetActiveApps();
            var appsDto = Mapper.Map<List<GameAppInfoDto>>(apps);

            foreach (var app in appsDto)
            {
                app.ActiveLobbies = await _lobbiesRepository.CountLobbiesWithGameId(app.Id);
            }
            return appsDto;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GameAppInfoDto>> GetGameAppInfo(int id)
        {
            var app = await _steamAppRepository.GetAppInfoAsync(id);

            if (app == null) return NotFound();

            var appDto = Mapper.Map<GameAppInfoDto>(app.Data);

            appDto.ActiveLobbies = await _lobbiesRepository.CountLobbiesWithGameId(app.Id);

            return appDto;
        }
    }
}