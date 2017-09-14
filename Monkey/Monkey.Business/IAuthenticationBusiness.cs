#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Interface </Project>
//     <File>
//         <Name> IAuthenticationBusiness.cs </Name>
//         <Created> 13/09/17 10:47:20 PM </Created>
//         <Key> 0b6435fb-3d3a-4fe1-ae17-a9703a45d61f </Key>
//     </File>
//     <Summary>
//         IAuthenticationBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Model.Models.User;
using System;
using System.Threading.Tasks;

namespace Monkey.Business
{
    public interface IAuthenticationBusiness : IBaseBusiness
    {
        Task<LoggedUserModel> SignInAsync(string username, string password);

        Task<LoggedUserModel> GetUserInfoByGlobalIdAsync(string globalId);

        Task<LoggedUserModel> GetUserInfoAsync(string refreshToken);

        Task SaveRefreshTokenAsync(int id, int clientId, string refreshToken, DateTimeOffset? expireOn);

        Task ExpireAllRefreshTokenAsync(int id);

        void CheckValidRefreshToken(string refreshToken, int clientId);

        string HashPassword(string password, out string salt);

        string HashPassword(string password, string salt);

        void CheckPasswordHash(string password, string passwordSalt, string passwordHash);
    }
}