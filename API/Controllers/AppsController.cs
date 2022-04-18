using API.DTOs.GameApps;
using API.DTOs.SteamApps;
using API.Entities.SteamApps;
using API.Extentions;
using API.Helpers.PaginationsParams;
using API.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        public AppsController(IMapper mapper, IMeilisearchService meilisearchService, IUnitOfWork unitOfWork) : base(mapper)
        {
            _unitOfWork = unitOfWork;
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
        public async Task<ActionResult<IEnumerable<GameAppInfoDto>>> GetActiveApps([FromQuery] UniversalParams universalParams)
        {
            var apps = await _unitOfWork.steamAppRepository.GetActiveApps(universalParams);

            Response.AddPaginationHeader(apps.CurrentPage, apps.PageSize, apps.TotalCount, apps.TotalPages);

            var appsDto = Mapper.Map<List<GameAppInfoDto>>(apps);

            foreach (var app in appsDto)
            {
                app.ActiveLobbies = await _unitOfWork.lobbiesRepository.CountActiveLobbiesWithGameId(app.Id);
            }
            return appsDto;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GameAppInfoDto>> GetGameAppInfo(int id)
        {
            var app = await _unitOfWork.steamAppRepository.GetAppInfoAsync(id);

            if (app == null) return NotFound();

            var appDto = Mapper.Map<GameAppInfoDto>(app.Data);

            appDto.ActiveLobbies = await _unitOfWork.lobbiesRepository.CountActiveLobbiesWithGameId(app.Id);

            return appDto;
        }
    }
}