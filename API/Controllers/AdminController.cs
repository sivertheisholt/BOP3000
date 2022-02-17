using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        public AdminController(IMapper mapper) : base(mapper)
        {
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public ActionResult GetUsersWithRoles()
        {
            return Ok("Only admins can see this");
        }

        [Authorize(Policy = "RequirePremiumRole")]
        [HttpGet("users-with-roles")]
        public ActionResult GetUsersWithPremium()
        {
            return Ok("Only premiums can see this");
        }

    }
}