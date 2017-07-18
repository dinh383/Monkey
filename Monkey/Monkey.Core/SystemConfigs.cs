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
    public static class SystemConfigs
    {
        /// <summary>
        ///     Production will read from key Production, else by MachineName 
        /// </summary>
        public static string DatabaseConnectionString { get; set; }

        /// <summary>
        ///     Log to File, Console 
        /// </summary>
        public static SerilogConfigModel Serilog { get; set; }   
        
        
        /// <summary>
        ///     Developer Config, API Document and Background Job
        /// </summary>
        public static DevelopersConfigModel Developers { get; set; }

        /// <summary>
        ///     Extra Server Info when Response created 
        /// </summary>
        public static ServerConfigModel Server { get; set; }

        /// <summary>
        ///     Identity Server - SSO - Scalable System 
        /// </summary>
        public static IdentityServerConfigModel IdentityServer { get; set; }

        /// <summary>
        ///     Redis Distributed Caching 
        /// </summary>
        public static RedisConfigModel Redis { get; set; }

        /// <summary>
        ///     Elastic Search Engine 
        /// </summary>
        public static ElasticConfigModel Elastic { get; set; }
    }
}