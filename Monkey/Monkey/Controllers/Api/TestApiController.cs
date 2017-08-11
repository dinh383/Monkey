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
        [HttpPost]
        [Route("")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(string), "Data")]
        public IActionResult Get([FromQuery]string data, [FromBody] TestData testData)
        {
            var a = int.Parse("a");
            return Ok(new
            {
                Data = "Sample Data"
            });
        }

        public class TestData
        {
            public string Data { get; set; }
        }
    }
}