#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IAuthenticationBusiness.cs </Name>
//         <Created> 17/09/17 2:42:36 PM </Created>
//         <Key> 595608e5-4d73-4836-bb7d-87978ee72832 </Key>
//     </File>
//     <Summary>
//         IAuthenticationBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;

namespace Monkey.Authentication.Interfaces
{
    public interface IAuthenticationBusiness
    {
        /// <summary>
        ///     Check userName and password is correct and user already active and not banned 
        /// </summary>
        /// <param name="userName"> </param>
        /// <param name="password"> </param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        void CheckValidSignInAsync(string userName, string password, string secretKey);

        /// <summary>
        ///     Generate refresh token and sign in 
        /// </summary>
        /// <param name="userName">     </param>
        /// <param name="clientSubject"></param>
        /// <param name="refreshToken"> </param>
        /// <returns> Subject of sign in user </returns>
        string SignIn(string userName, int clientSubject, out string refreshToken);

        void ExpireAllRefreshToken(string subject);

        void CheckValidRefreshToken(int clientSubject, string refreshToken);

        string HashPassword(string password, DateTimeOffset hashTime, string secretKey);
    }
}