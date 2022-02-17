using API.DTOs.SteamApps;
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
        public AppsController(IMapper mapper, IMeilisearchService meilisearchService) : base(mapper)
        {
            _meilisearchService = meilisearchService;
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AppListInfoDto>>> SearchForApp(string name, int limit = 10)
        {
            var hits = await _meilisearchService.SearchForAppAsync(name, new SearchQuery { Limit = limit });
            return Ok(Mapper.Map<IEnumerable<AppListInfoDto>>(hits.Hits));
        }
    }
}