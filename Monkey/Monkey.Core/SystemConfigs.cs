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

using Monkey.Core.Models.Config;

namespace Monkey.Core
{
    /// <summary>
    ///     Setting configuration sync from appsettings.json of Main project 
    /// </summary>
    public static class SystemConfigs
    {
        /// <summary>
        ///     Production, Staging will read from key Environment Name, else by MachineName 
        /// </summary>
        public static string DatabaseConnectionString { get; set; }

        /// <summary>
        ///     Folder Name of wwwroot, Areas and Areas name and request path Config 
        /// </summary>
        public static MvcPathConfigModel MvcPath { get; set; } = new MvcPathConfigModel();

        public static PagedCollectionParametersConfigModel PagedCollectionParameters { get; set; } = new PagedCollectionParametersConfigModel();

        /// <summary>
        ///     Current System Identity Information 
        /// </summary>
        public static IdentityModel Identity { get; set; } = new IdentityModel();
    }
}