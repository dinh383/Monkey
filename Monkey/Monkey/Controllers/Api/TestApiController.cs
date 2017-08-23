using Microsoft.AspNetCore.Mvc;
using Monkey.ViewModels.Api;
using Puppy.Logger;
using Puppy.Logger.Core.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;

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
    }
}