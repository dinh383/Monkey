using Microsoft.AspNetCore.Mvc;
using Monkey.Areas.Portal.Hubs;
using Monkey.Auth.Filters.Attributes;
using Puppy.Web.HttpUtils;

namespace Monkey.Areas.Api.Controllers
{
    [Route(Endpoint)]
    public class TestController : ApiController
    {
        public const string Endpoint = AreaName + "/test";

        private readonly NotificationHub _notificationHub;

        public TestController(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }

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

        /// <summary>
        ///     Send Notification 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("send-notification")]
        [Auth]
        public IActionResult SendNotification(string message)
        {
            _notificationHub.NotificationUserAsync(Core.LoggedInUser.Current.Subject, message);
            return Ok();
        }
    }
}