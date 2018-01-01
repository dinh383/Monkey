#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> SystemConfigs.cs </Name>
//         <Created> 17/07/17 8:42:29 PM </Created>
//         <Key> 671063d3-f67d-403b-8836-4f4f0a8bc42d </Key>
//     </File>
//     <Summary>
//         SystemConfigs.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Configs.Models;
using System;

namespace Monkey.Core.Configs
{
    /// <summary>
    ///     Setting configuration sync from appsettings.json of Main project 
    /// </summary>
    public static class SystemConfig
    {
        public static DateTimeOffset SystemVersion { get; set; }

        /// <summary>
        ///     Production, Staging will read from key Environment Name, else by MachineName 
        /// </summary>
        public static string DatabaseConnectionString { get; set; }

        /// <summary>
        ///     Folder Name of wwwroot, Areas and Areas name and request path Config 
        /// </summary>
        public static MvcPathConfigModel MvcPath { get; set; } = new MvcPathConfigModel();

        /// <summary>
        ///     Config use datetime with TimeZone. Default is "UTC", See more: https://msdn.microsoft.com/en-us/library/gg154758.aspx 
        /// </summary>
        public static string SystemTimeZone { get; set; } = "UTC";

        /// <summary>
        ///     [Auto Reload] 
        /// </summary>
        public static PagedCollectionParametersConfigModel PagedCollectionParameters { get; set; } = new PagedCollectionParametersConfigModel();

        /// <summary>
        ///     [Auto Reload] 
        /// </summary>
        public static SendGridConfigModel SendGrid { get; set; } = new SendGridConfigModel();
    }
}