#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> IdentityServerExtensions.cs </Name>
//         <Created> 31/07/17 10:42:08 PM </Created>
//         <Key> 410a024e-dd83-40c7-b482-1e67a94ded9f </Key>
//     </File>
//     <Summary>
//         IdentityServerExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Monkey.Extensions
{
    public static class IdentityServerExtensions
    {
        /// <summary>
        ///     [Security] Identity Server 
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseIdentityServerMonkey(this IApplicationBuilder app)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = Core.SystemConfigs.IdentityServer.ConnectionString,
                ApiName = Core.SystemConfigs.IdentityServer.ApiName,
                EnableCaching = true,
                CacheDuration = TimeSpan.FromMinutes(10),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false
            });

            return app;
        }
    }
}