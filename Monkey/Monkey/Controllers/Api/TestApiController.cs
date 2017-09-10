using Microsoft.AspNetCore.Mvc;
using Monkey.Core;
using Monkey.Filters.Authorize;
using Puppy.Web;

namespace Monkey.Controllers.Api
{
    [Route("api/test")]
    public class TestApiController : ApiController
    {
        /// <summary>
        ///     Test 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Authorize(Enums.Permission.Admin)]
        [Authorize(Enums.Permission.Developer)]
        public IActionResult Test()
        {
            var deviceInfo = HttpContext.Request.GetDeviceInfo();
            return Ok(deviceInfo);
        }
    }
}