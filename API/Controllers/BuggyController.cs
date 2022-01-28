using API.Data;
using API.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// BuggyController is only used for development
    /// </summary>
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        /// <summary>
        /// Constructs a new BuggyController
        /// </summary>
        /// <param name="context"></param>
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Auth test
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        /// <summary>
        /// Not Found
        /// </summary>
        /// <returns>This will always return a 404 status code</returns>
        [Authorize]
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            // Find user at -1 - for null
            var thing = _context.Users.Find(-1);

            // Return 404
            if (thing == null) return NotFound();

            return Ok(thing);
        }

        /// <summary>
        /// Server Error
        /// </summary>
        /// <returns>This will always return a 500 Internal Server Error
        /// </returns>
        [Authorize]
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        /// <summary>
        /// Bad Request
        /// </summary>
        /// <returns>This will always return a 400 Bad Request</returns>
        [Authorize]
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}