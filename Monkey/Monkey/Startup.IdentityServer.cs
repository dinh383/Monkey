using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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

                //application to use cookie authentication
                //app.UseCookieAuthentication(new CookieAuthenticationOptions
                //{
                //    AuthenticationScheme = Constants.Setting.CookieSchemaName,
                //    AutomaticAuthenticate = true,
                //    AutomaticChallenge = true,
                //    LoginPath = new PathString("/Account/Login"),
                //    LogoutPath = new PathString("/Account/Login"),
                //    AccessDeniedPath = new PathString("/Account/Forbidden"),
                //    SlidingExpiration = true,
                //    ExpireTimeSpan = TimeSpan.FromDays(30)
                //});

                app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
                {
                    Authority = ConfigurationRoot.GetValue<string>("IdentityServer:ConnectionString"),
                    ApiName = "Monkey_Api",
                    EnableCaching = true,
                    CacheDuration = TimeSpan.FromMinutes(10),
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true,
                    RequireHttpsMetadata = false
                });

                ////use OpenID Connect Provider (IdentityServer)
                //app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
                //{
                //    RequireHttpsMetadata = false,
                //    AuthenticationScheme = IdentityServerConstants.ProtocolTypes.OpenIdConnect,
                //    GetClaimsFromUserInfoEndpoint = true,
                //    SignInScheme = Constants.Setting.CookieSchemaName,

                //    Authority = ConfigurationRoot.GetValue<string>("IdentityServerUrl"),
                //    PostLogoutRedirectUri = "/",
                //    AutomaticAuthenticate = true,
                //    ClientId = "Monkey_api",
                //    ClientSecret = "Monkeyapi",
                //    DisplayName = "Monkey API",
                //    // required if you want to return a 403 and not a 401 for forbidden responses
                //    AutomaticChallenge = true,
                //    SaveTokens = true,
                //    Scope =
                //    {
                //        "Monkey_api",
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.OfflineAccess
                //    }
                //});
            }
        }
    }
}