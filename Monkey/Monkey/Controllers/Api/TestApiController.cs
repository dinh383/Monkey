using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace Monkey.Controllers.Api
{
    [Route("api/test")]
    public class TestApiController : ApiController
    {
        /// <summary>
        ///     Test ExceptionInfo 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("exception")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public IActionResult TestException()
        {
            int nAn = int.Parse("Not a Number");
            return NoContent();
        }
    }
}