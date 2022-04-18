using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// FallbackController is used as fallback for the client
    /// </summary>
    public class FallbackController : BaseApiController
    {
        public FallbackController(IMapper mapper) : base(mapper)
        {
        }

        public ActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot", "index.html"), "text/HTML");
        }
    }
}