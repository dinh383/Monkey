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

using Monkey.Core.Constants;
using Monkey.Core.Models.User;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Business.User
{
    public interface IUserBusiness : IBaseBusiness
    {
        void CheckExistsById(params int[] ids);

        void CheckExistsBySubject(params string[] subjects);

        void CheckUniqueUserName(string userName, int? excludeId = null);

        void CheckExistByUserName(string userName);

        void CheckExistByEmail(string email);

        void CheckUniqueEmail(string email, int? excludeId = null);

        void CheckUniquePhone(string phone, int? excludeId = null);

        void CheckExistByPhone(string phone);

        /// <summary>
        ///     Get list user id have all params permissions 
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        List<int> ListUserIdByPermissions(params Enums.Permission[] permissions);

        Task<CreateUserResultModel> CreateUserByEmailAsync(string email, int? roleId, string fullName, CancellationToken cancellationToken = default(CancellationToken));

        Task<CreateUserResultModel> CreateOrGetUserByEmailAsync(string email, int? roleId, string fullName, CancellationToken cancellationToken = default(CancellationToken));

        Task<CreateUserResultModel> CreateUserByPhoneAsync(string phone, int? roleId, string fullName, CancellationToken cancellationToken = default(CancellationToken));

        Task<CreateUserResultModel> CreateOrGetUserByPhoneAsync(string phone, int? roleId, string fullName, CancellationToken cancellationToken = default(CancellationToken));

        Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

        Task<UserModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

        Task<UserModel> GetByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken));

        Task<UserModel> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default(CancellationToken));

        Task<UserModel> GetBySubjectAsync(string subject, CancellationToken cancellationToken = default(CancellationToken));

        Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(UserUpdateModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateProfileAsync(UpdateProfileModel model, CancellationToken cancellationToken = default(CancellationToken));
    }
}