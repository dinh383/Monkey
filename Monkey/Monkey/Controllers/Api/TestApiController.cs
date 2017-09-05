using Microsoft.AspNetCore.Mvc;
using Monkey.Authentication;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace Monkey.Controllers.Api
{
    [Route("api/test")]
    public class TestApiController : ApiController
    {
        /// <summary>
        ///     Test Exception 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("exception")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public IActionResult TestException()
        {
            var token = TokenHelper.GenerateAccessToken(new
            {
                userId = 123456,
                userName = "tonguyen"
            });

            var a = TokenHelper.GetTokenData<object>(token.AccessToken);

            bool isAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

            int nAn = int.Parse("Not a Number");
            return NoContent();
        }
    }
}