using Microsoft.AspNetCore.Mvc;
using Monkey.Filters.Authorize;
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
        [Route("/test/deviceinfo")]
        public IActionResult DeviceInfo()
        {
            var deviceInfo = HttpContext.Request.GetDeviceInfo();
            return Ok(deviceInfo);
        }

        /// <summary>
        ///     Logged In User 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/test/userinfo")]
        [Authorize]
        public IActionResult LoggedInUser()
        {
            return Ok(new
            {
                IsAuthenticated = HttpContext.User.Identity.IsAuthenticated,
                UserInfo = Core.LoggedInUser.Current
            });
        }
    }
}