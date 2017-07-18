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
        ///     System Time Zone Info, Can Find Full list ID via Current Machine System
        /// </summary>
        /// <remarks>System store list Time Zone Info in <c>Regedit Key</c>: "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones"</remarks>
        public string TimeZoneId { get; set; }

        /// <summary>
        ///     Log to File, Console 
        /// </summary>
        public SerilogConfigModel Serilog { get; set; }

        /// <summary>
        ///     Extra Server Info when Response created 
        /// </summary>
        public ServerConfigModel Server { get; set; }

        /// <summary>
        /// Identity Server - SSO - Scalable System
        /// </summary>
        public IdentityServerConfigModel IdentityServer { get; set; }

        /// <summary>
        ///     Redis Distributed Caching 
        /// </summary>
        public RedisConfigModel Redis { get; set; }

        /// <summary>
        ///     Elastic Search Engine 
        /// </summary>
        public ElasticConfigModel Elastic { get; set; }
    }
}