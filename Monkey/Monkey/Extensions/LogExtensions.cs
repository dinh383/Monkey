#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> LogExtensions.cs </Name>
//         <Created> 31/07/17 10:42:17 PM </Created>
//         <Key> 4f9a470a-f01d-4ac7-b4e8-6f3330dcc13a </Key>
//     </File>
//     <Summary>
//         LogExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Monkey.Extensions
{
    public static class LogExtensions
    {
        /// <summary>
        ///     [Log] Serilog 
        /// </summary>
        /// <param name="app">              </param>
        /// <param name="loggerFactory">    </param>
        /// <param name="configurationRoot"></param>

        public static IApplicationBuilder UseLogMonkey(this IApplicationBuilder app, ILoggerFactory loggerFactory, IConfigurationRoot configurationRoot)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configurationRoot).CreateLogger();
            loggerFactory.AddSerilog();

            return app;
        }
    }
}