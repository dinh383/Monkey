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

using Monkey.Core.ConfigModels;

namespace Monkey.Core
{
    /// <summary>
    ///     Setting configuration sync from appsettings.json of Main project 
    /// </summary>
    public class SystemConfigs
    {
        /// <summary>
        ///     Production will read from key Production, else by MachineName 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     System Time Zone Info, Can Find Full list ID via
        ///     "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones"
        /// </summary>
        public string TimeZoneId { get; set; }

        /// <summary>
        ///     Log to File, Console 
        /// </summary>
        public SerilogConfigModel Serilog { get; set; }
    }
}