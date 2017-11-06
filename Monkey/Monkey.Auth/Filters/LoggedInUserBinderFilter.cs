#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> BindingLoggedInUserFilter.cs </Name>
//         <Created> 13/10/17 8:30:53 PM </Created>
//         <Key> 5b9decd3-ded3-471f-8477-d0a0050d923d </Key>
//     </File>
//     <Summary>
//         BindingLoggedInUserFilter.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Mvc.Filters;

namespace Monkey.Auth.Filters
{
    public class LoggedInUserBinderFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            LoggedInUserBinder.BindLoggedInUser(context.HttpContext);
        }
    }
}