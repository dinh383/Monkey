using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monkey.Core.Models;
using Monkey.Core.Models.Log;
using Newtonsoft.Json;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;
using Puppy.Logger;
using Puppy.Logger.Core.Models;
using Puppy.Web;
using Puppy.Web.Models.Api;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Monkey.Areas.Api.Controllers
{
    [Route(Endpoint)]
    [AllowAnonymous]
    public class LogController : ApiController
    {
        public const string Endpoint = AreaName + "/log";
        public const string DataLogEndpoint = "changes";
        public const string ExceptionEndpoint = "exceptions";

        /// <summary>
        ///     View Data Change Log 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DataLogEndpoint)]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(List<DataLogModel>))]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public IActionResult GetActivityLog([FromQuery]PagedCollectionParametersModel model)
        {
            Expression<Func<LogEntity, bool>> predicate = x => x.Type == "DataChange";

            var termsNormalize = StringHelper.Normalize(model.Terms);

            if (!string.IsNullOrWhiteSpace(termsNormalize))
            {
                predicate = x => x.Message.ToUpperInvariant().Contains(termsNormalize)
                                 || x.CreatedTime.ToString(Puppy.Core.Constants.StandardFormat.DateTimeOffSetFormat).Contains(termsNormalize);
            }

            var logs = Log.Get(out long total, predicate: predicate, orders: x => x.CreatedTime, isOrderByDescending: true, skip: model.Skip, take: model.Take);

            var listActivity = logs.Select(x => JsonConvert.DeserializeObject<DataLogModel>(x.Message)).ToList();

            var listActivityModel = listActivity.MapTo<List<DataLogModel>>();

            var pagedCollectionResult = new PagedCollectionResultModel<DataLogModel>
            {
                Skip = model.Skip,
                Take = model.Take,
                Terms = model.Terms,
                Total = total,
                Items = listActivityModel
            };

            if (pagedCollectionResult.Total <= 0)
            {
                return NoContent();
            }

            var responseData = Url.GeneratePagedCollectionResult(pagedCollectionResult);

            return Ok(responseData);
        }

        /// <summary>
        ///     View Exception Log 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ExceptionEndpoint)]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(List<LogEntity>))]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public IActionResult GetExceptionLog([FromQuery]PagedCollectionParametersModel model)
        {
            Expression<Func<LogEntity, bool>> predicate = x => x.Type == Log.LogTypeException;

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