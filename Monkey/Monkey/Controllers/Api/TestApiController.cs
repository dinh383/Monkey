using Microsoft.AspNetCore.Mvc;
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
        [Route("")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public IActionResult Get()
        {
            int a = int.Parse("Not a Number");
            return NoContent();
        }
    }
}