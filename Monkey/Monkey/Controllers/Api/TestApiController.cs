using Microsoft.AspNetCore.Mvc;
using Puppy.Logger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
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
            int nAn = int.Parse("Not a Number");
            return NoContent();
        }

        /// <summary>
        ///     Test Get Log 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("logs")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public IActionResult GetLogs()
        {
            var logs = Log.Get();
            return Ok(new
            {
                Data = logs
            });
        }
    }
}