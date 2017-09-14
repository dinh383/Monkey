using Microsoft.AspNetCore.Mvc;
using Puppy.Web;

namespace Monkey.Controllers.Api
{
    [Route(Endpoint)]
    public class TestApiController : ApiController
    {
        public const string Endpoint = AreaName + "/test";

        /// <summary>
        ///     Device Info 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IActionResult Test()
        {
            var deviceInfo = HttpContext.Request.GetDeviceInfo();
            return Ok(deviceInfo);
        }
    }
}