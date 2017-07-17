using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Monkey
{
    public partial class Startup
    {
        public static class Log
        {
            public static void Middleware(IApplicationBuilder app, ILoggerFactory loggerFactory)
            {
                Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(ConfigurationRoot).CreateLogger();
                loggerFactory.AddSerilog();
            }
        }
    }
}