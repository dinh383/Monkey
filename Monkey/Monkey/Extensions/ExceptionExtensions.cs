#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ExceptionExtensions.cs </Name>
//         <Created> 31/07/17 10:41:47 PM </Created>
//         <Key> 4c9bb375-e33c-43f6-9edf-6eaab723ccc3 </Key>
//     </File>
//     <Summary>
//         ExceptionExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using Puppy.Core.EnvironmentUtils;

namespace Monkey.Extensions
{
    public static class ExceptionExtensions
    {
        public static IApplicationBuilder UseExceptionMonkey(this IApplicationBuilder app)
        {
            if (EnvironmentHelper.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            return app;
        }
    }
}