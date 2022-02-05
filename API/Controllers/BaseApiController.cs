using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// BaseApiController is the base for all other controllers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Gets the userid from the claims
        /// </summary>
        /// <returns>int: The user ID</returns>
        protected int GetUserIdFromClaim()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            return Int32.Parse(userId);
        }
    }
}