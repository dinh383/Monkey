using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace Monkey.Controllers.Api
{
    [Route("api/test")]
    public class TestApiController : ApiController
    {
        /// <summary>
        ///     Test Get 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(string), "Data")]
        public IActionResult Get()
        {
            return Ok(new
            {
                Data = "Sample Data"
            });
        }
    }
}