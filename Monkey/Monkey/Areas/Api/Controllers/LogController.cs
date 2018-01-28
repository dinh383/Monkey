using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monkey.Core.Models;
using Puppy.Core.StringUtils;
using Puppy.Logger;
using Puppy.Logger.Core.Models;
using Puppy.Web;
using Puppy.Web.Models.Api;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Monkey.Areas.Api.Controllers
{
    [Route(Endpoint)]
    [AllowAnonymous]
    public class LogController : ApiController
    {
        public const string Endpoint = AreaName + "/logs";
        public const string GetEndpoint = "";

        /// <summary>
        ///     View Logs 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(GetEndpoint)]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(List<LogEntity>))]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public IActionResult Get([FromQuery]PagedCollectionParametersModel model)
        {
            Expression<Func<LogEntity, bool>> predicate = x => true;

            var termsNormalize = StringHelper.Normalize(model.Terms);

            if (!string.IsNullOrWhiteSpace(termsNormalize))
            {
                predicate = x => x.Id.ToUpperInvariant().Contains(termsNormalize)
                                 || x.Message.ToUpperInvariant().Contains(termsNormalize)
                                 || x.Level.ToString().ToUpperInvariant().Contains(termsNormalize)
                                 || x.CreatedTime.ToString(Puppy.Core.Constants.StandardFormat.DateTimeOffSetFormat).Contains(termsNormalize);
            }

            var logs = Log.Get(out long total, predicate: predicate, orders: x => x.CreatedTime, isOrderByDescending: true, skip: model.Skip, take: model.Take);

            var pagedCollectionResult = new PagedCollectionResultModel<LogEntity>
            {
                Skip = model.Skip,
                Take = model.Take,
                Terms = model.Terms,
                Total = total,
                Items = logs
            };

            if (pagedCollectionResult.Total <= 0)
            {
                return NoContent();
            }

            var responseData = Url.GeneratePagedCollectionResult(pagedCollectionResult);

            return Ok(responseData);
        }
    }
}