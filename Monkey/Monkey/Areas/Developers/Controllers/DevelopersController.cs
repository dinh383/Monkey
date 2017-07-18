using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy.Core.StringUtils;

namespace Monkey.Areas.Developers.Controllers
{
    [Route("Developers")]
    public class DevelopersController : DevelopersMvcController
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public DevelopersController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index(string key)
        {
            key = StringHelper.ReplaceNullOrWhiteSpaceToEmpty(key);

            var documentName = Core.SystemConfigs.Developers.ApiDocumentName;
            var documentApiBaseUrl = Core.SystemConfigs.Developers.ApiDocumentUrl + documentName;
            var documentJsonFileName = Core.SystemConfigs.Developers.ApiDocumentJsonFile;
            var documentUrlBase = documentApiBaseUrl.Replace(documentName, string.Empty).TrimEnd('/');
            var swaggerEndpoint = $"{documentUrlBase}/{documentName}/{documentJsonFileName}" + "?key=" + key;
            ViewBag.ApiDocumentPath = $"{_contextAccessor.HttpContext.Request.Scheme }://{_contextAccessor.HttpContext.Request.Host.Value}{swaggerEndpoint}";
            ViewBag.ApiKey = key;

            return View();
        }

        [Route("Viewer")]
        [HttpGet]
        public IActionResult Viewer(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                key = string.Empty;
            ViewBag.ApiKey = key;
            return View();
        }
    }
}