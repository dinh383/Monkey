using Microsoft.AspNetCore.Mvc;
using Monkey.Areas.Developers.Controllers.Base;
using Monkey.Model.Models;
using Puppy.Logger;
using Puppy.Logger.Core.Models;
using Puppy.Logger.Filters;
using Puppy.Swagger;
using Puppy.Swagger.Filters;
using Puppy.Web;
using Puppy.Web.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Net;

namespace Monkey.Areas.Developers.Controllers
{
    [Route(Constants.Endpoint.DevelopersArea.Developers)]
    public class DevelopersController : DevelopersMvcController
    {
        #region API DOC

        [HideInDocs]
        [ServiceFilter(typeof(ApiDocAccessFilter))]
        [Route("")]
        [HttpGet]
        public IActionResult Index() => Helper.GetApiDocHtml(Url,
            Url.AbsoluteAction(nameof(JsonViewer), Constants.Endpoint.DevelopersArea.Developers,
                new { area = Constants.Endpoint.DevelopersArea.Root }));

        [HideInDocs]
        [ServiceFilter(typeof(ApiDocAccessFilter))]
        [HttpGet]
        [Route("json-viewer")]
        public IActionResult JsonViewer() => Helper.GetApiJsonViewerHtml(Url);

        #endregion API DOC

        #region Log

        /// <summary>
        ///     Logs 
        /// </summary>
        /// <param name="pagedCollectionParametersModel"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Logger write Log with `message queue` so when create a log, it **near real-time
        ///     log**. Terms search for `Id`, `Message`, `Level`, `CreatedTime` (with string format
        ///     is `yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK`, ex: "2017-08-24T00:56:29.6271125+07:00")
        /// </remarks>
        [ServiceFilter(typeof(ViewLogViaUrlAccessFilter))]
        [HttpGet]
        [Route("logs")]
        [Produces(ContentType.Json, ContentType.Xml)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ICollection<LogEntity>))]
        public IActionResult Logs([FromQuery]PagedCollectionParametersModel pagedCollectionParametersModel)
            => Log.GetLogsContentResult(Url, pagedCollectionParametersModel.Skip, pagedCollectionParametersModel.Take, pagedCollectionParametersModel.Terms);

        /// <summary>
        ///     Log 
        /// </summary>
        /// <param name="id"> Id should be a `guid string` with format [**"N"**](https://goo.gl/pYVXKd) </param>
        /// <returns></returns>
        /// <remarks>
        ///     <para>
        ///         Logger write Log with `message queue` so when create a log, it **near real-time log**
        ///     </para>
        /// </remarks>
        [ServiceFilter(typeof(ViewLogViaUrlAccessFilter))]
        [HttpGet]
        [Route("logs/{id}")]
        [Produces(ContentType.Json, ContentType.Xml)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(LogEntity))]
        public IActionResult SingleLog([FromRoute]string id) => Log.GetLogContentResult(HttpContext, id);

        #endregion Log
    }
}