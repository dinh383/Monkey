using Microsoft.AspNetCore.Builder;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Monkey
{
    public partial class Startup
    {
        public static class IdentityServer
        {
            public static void Middleware(IApplicationBuilder app)
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
            }
        }
    }
}