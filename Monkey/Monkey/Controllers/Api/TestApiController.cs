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

        private const string LogEndpointPattern = "logs/{skip:int}/{take:int}";

        /// <summary>
        ///     Test Get Log 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(LogEndpointPattern)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public IActionResult GetLogs([FromRoute]int skip, [FromRoute]int take, [FromQuery]string terms)
        {
            Expression<Func<LogEntity, bool>> predicate = null;

            if (!string.IsNullOrWhiteSpace(terms))
            {
                predicate = x => x.Message.Contains(terms);
            }

            var logs = Log.Get(out long total, predicate: predicate, orders: x => x.CreatedTime, isOrderByDescending: true, skip: skip, take: take);

            if (total <= 0)
            {
                // Return 204 for No Data Case
                return NoContent();
            }

            var placeholderLinkView = PlaceholderLinkViewModel.ToCollection(LogEndpointPattern, HttpMethod.Get.Method, new { skip, take, terms });
            var collectionFactoryViewModel = new PagedCollectionFactoryViewModel<LogEntity>(placeholderLinkView, LogEndpointPattern);
            var collectionViewModel = collectionFactoryViewModel.CreateFrom(logs, skip, take, total);
            return Ok(collectionViewModel);
        }
    }
}