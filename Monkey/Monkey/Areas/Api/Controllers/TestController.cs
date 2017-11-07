using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters.Attributes;
using Puppy.Web.HttpUtils;

namespace Monkey.Areas.Api.Controllers
{
    [Route(Endpoint)]
    public class TestController : ApiController
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