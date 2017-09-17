#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IUserBusiness.cs </Name>
//         <Created> 17/09/17 3:05:28 PM </Created>
//         <Key> 5d685857-d78f-4296-989e-66b41335c4b0 </Key>
//     </File>
//     <Summary>
//         IUserBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System.Threading.Tasks;

namespace Monkey.Authentication.Interfaces
{
    public interface IUserBusiness
    {
        void CheckExists(params string[] userNames);

        Task<string> GetSubjectByRefreshTokenAsync(string refreshToken);
    }
}