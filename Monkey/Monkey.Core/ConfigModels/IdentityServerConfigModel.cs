#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> IdentityServerConfigModel.cs </Name>
//         <Created> 18/07/17 12:01:42 PM </Created>
//         <Key> a0fb5a2f-81b9-4c25-8f49-08c23f61e831 </Key>
//     </File>
//     <Summary>
//         IdentityServerConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;

namespace Monkey.Core.ConfigModels
{
    public class IdentityServerConfigModel
    {
        /// <summary>
        ///     Root Endpoint (Domain) of Identity Server (SSO) 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Api Name (Audience Name - Name of this system register with SSO) 
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        ///     Cache Duration for Request send to Identity Server 
        /// </summary>
        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromMinutes(10);
    }
}