using Monkey.Auth.Filters.Attributes;
using Microsoft.AspNetCore.Mvc;
using Puppy.Web.HttpUtils;

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
        [Route("deviceinfo")]
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
        [Route("userinfo")]
        [Auth]
        public IActionResult LoggedInUser()
        {
            return Ok(new
            {
                HttpContext.User.Identity.IsAuthenticated,
                UserInfo = Core.LoggedInUser.Current
            });
        }
    }
}