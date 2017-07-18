using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Monkey.Areas.Developers
{
    public static class DeveloperHelper
    {
        public static bool IsCanAccess(HttpContext httpContext, string key)
        {
            try
            {
                string requestKey = httpContext.Request.Query["key"];
                var isCanAccess = string.IsNullOrWhiteSpace(key) || key == requestKey;
                return isCanAccess;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsCanAccess(HttpContext httpContext, IConfigurationRoot configurationRoot)
        {
            string accessKey = configurationRoot.GetValue<string>("Developers:AccessKey");
            return IsCanAccess(httpContext, accessKey);
        }
    }
}