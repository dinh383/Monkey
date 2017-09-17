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

using System.Threading.Tasks;

namespace Monkey.Business
{
    public interface IAuthenticationBusiness : IBaseBusiness, Authentication.Interfaces.IAuthenticationBusiness
    {
        /// <summary>
        ///     Active user via email, setup new username and password 
        /// </summary>
        /// <param name="subject">    </param>
        /// <param name="newUserName"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task ActiveByEmailAsync(string subject, string newUserName, string newPassword);

        Task ActiveByPhoneAsync(string subject, string newUserName, string newPassword);

        /// <summary>
        ///     Create User and Return Subject of new user 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<string> CreateUserAsync(string email);
    }
}