#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Interface </Project>
//     <File>
//         <Name> IAuthenticationService.cs </Name>
//         <Created> 13/09/17 10:47:07 PM </Created>
//         <Key> 96a454c5-15eb-425c-810e-fd9f18cf2c54 </Key>
//     </File>
//     <Summary>
//         IAuthenticationService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Authentication.Models;
using System.Threading.Tasks;

namespace Monkey.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AccessTokenModel> GetTokenAsync(IRequestTokenModel model);

        Task<T> GetLoggedInUserAsync<T>(string accessToken) where T : class, ILoggedInUserModel, new();

        Task ExpireAllRefreshTokenAsync(string accessToken);
    }
}