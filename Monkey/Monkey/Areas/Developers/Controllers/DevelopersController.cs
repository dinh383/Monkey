using Microsoft.AspNetCore.Mvc;
using Monkey.Areas.Developers.Controllers.Base;
using Puppy.Logger;
using Puppy.Logger.Filters;
using Puppy.Swagger;
using Puppy.Swagger.Filters;
using Puppy.Web;

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

        public const string LogsEndpointPattern = "logs/{skip:int}/{take:int}";

        /// <summary>
        ///     Logs 
        /// </summary>
        /// <param name="skip"> </param>
        /// <param name="take"> </param>
        /// <param name="terms">
        ///     terms do contains search for Id, Message, Level, CreatedTime (with string format is
        ///     "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", ex: "2017-08-24T00:56:29.6271125+07:00")
        /// </param>
        /// <returns></returns>
        [ServiceFilter(typeof(ViewLogViaUrlAccessFilter))]
        [HttpGet]
        [Route(LogsEndpointPattern)]
        [Produces(ContentType.Json, ContentType.Xml)]
        public IActionResult Logs([FromRoute]int skip, [FromRoute]int take, [FromQuery]string terms)
        {
            // Base on "httpContext" will return ContentType XML when Request Header Accept or
            // ContentType is XML, else return ContentType Json
            return Log.GetLogsContentResult(HttpContext, LogsEndpointPattern, skip, take, terms);
        }

        /// <summary>
        ///     Log 
        /// </summary>
        /// <param name="id"> Id should be a guid string with format "N" </param>
        /// <returns></returns>
        [ServiceFilter(typeof(ViewLogViaUrlAccessFilter))]
        [HttpGet]
        [Route("logs/{id}")]
        [Produces(ContentType.Json, ContentType.Xml)]
        public IActionResult SingleLog([FromRoute]string id)
        {
            // Base on "httpContext" will return ContentType XML when Request Header Accept or
            // ContentType is XML, else return ContentType Json
            return Log.GetLogContentResult(HttpContext, id);
        }

        #endregion Log
    }
}