#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Interface </Project>
//     <File>
//         <Name> IUserBusiness.cs </Name>
//         <Created> 18/07/17 4:49:26 PM </Created>
//         <Key> 1a8c0357-4f32-42de-ade4-851e33d3adc2 </Key>
//     </File>
//     <Summary>
//         IUserBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models.User;
using System;
using System.Threading.Tasks;

namespace Monkey.Business
{
    public interface IUserBusiness : IBaseBusiness
    {
        Task<int> GetTotalAsync();

        // CHECK

        void CheckExists(params int[] ids);

        void CheckExists(params string[] userNames);

        void CheckActives(params string[] userNames);

        void CheckExistsBySubject(params string[] globalIds);

        // CREATE

        Task<string> CreateAsync(string email);

        // ACTIVE

        Task ActiveByEmailAsync(string globalId, string userName, string passwordHash, DateTimeOffset updatedTime);

        Task ActiveByPhoneAsync(string globalId, string userName, string passwordHash, DateTimeOffset updatedTime);

        // GET

        Task<LoggedInUserModel> GetUserInfoBySubjectAsync(string globalId);

        Task<string> GetUserSubjectByRefreshTokenAsync(string refreshToken);
    }
}