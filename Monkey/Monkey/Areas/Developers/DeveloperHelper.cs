using Microsoft.AspNetCore.Http;
using Monkey.Core;

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

        public static bool IsCanAccess(HttpContext httpContext)
        {
            return IsCanAccess(httpContext, SystemConfigs.Developers.AccessKey);
        }
    }
}