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

using Monkey.Core.Models.Auth;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using System.Threading.Tasks;

namespace Monkey.Business.Auth
{
    public interface IUserBusiness : IBaseBusiness
    {
        void CheckExistsById(params int[] ids);

        void CheckExistsBySubject(params string[] subjects);

        void CheckUniqueUserName(string userName, int? excludeId = null);

        void CheckExistByUserName(string userName);

        void CheckUniqueEmail(string email, int? excludeId = null);

        void CheckUniquePhone(string phone, int? excludeId = null);

        /// <summary>
        ///     Create User and Return Subject of new user 
        /// </summary>
        /// <param name="email"> </param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<string> CreateUserByEmailAsync(string email, int? roleId);

        /// <summary>
        ///     Active user via email, setup new username and password 
        /// </summary>
        /// <param name="subject">    </param>
        /// <param name="newUserName"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task ActiveByEmailAsync(string subject, string newUserName, string newPassword);

        Task ActiveByPhoneAsync(string subject, string newUserName, string newPassword);

        Task RemoveAsync(int id);

        Task<UserModel> GetAsync(int id);

        Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model);

        Task UpdateAsync(UserUpdateModel model);
    }
}