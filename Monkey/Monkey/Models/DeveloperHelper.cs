using Microsoft.AspNetCore.Http;

namespace Monkey.Models
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
    }
}