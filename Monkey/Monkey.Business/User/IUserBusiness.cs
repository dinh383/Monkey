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

using System.Collections.Generic;
using Monkey.Core.Models.User;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using System.Threading;
using System.Threading.Tasks;
using Monkey.Core.Constants;
using Monkey.Core.Models;
using Puppy.Web.Models.Api;

namespace Monkey.Business.User
{
    public interface IUserBusiness : IBaseBusiness
    {
        #region Get

        Task<DataTableResponseDataModel<UserModel>> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default);

        Task<UserModel> GetAsync(int id, CancellationToken cancellationToken = default);

        Task<UserModel> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<UserModel> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

        Task<UserModel> GetBySubjectAsync(string subject, CancellationToken cancellationToken = default);

        List<UserLookupModel> GetListLookupByPermissions(params Enums.Permission[] permissions);

        Task<PagedCollectionResultModel<UserLookupModel>> GetListLookupByPermissionsAsync(
            PagedCollectionParametersModel model, CancellationToken cancellationToken = default,
            params Enums.Permission[] permissions);

        #endregion

        #region Create

        Task<CreateUserResultModel> CreateAsync(CreateUserModel model, CancellationToken cancellationToken = default);

        Task<CreateUserResultModel> CreateOrGetAsync(CreateUserModel model, CancellationToken cancellationToken = default);

        #endregion

        #region Update

        Task UpdateAsync(UpdateUserModel model, CancellationToken cancellationToken = default);

        Task UpdateProfileAsync(UpdateProfileModel model, CancellationToken cancellationToken = default);

        #endregion

        #region Remove

        Task RemoveAsync(int id, CancellationToken cancellationToken = default);

        #endregion

        #region Validation

        void CheckExistsById(params int[] ids);

        void CheckExistsBySubject(params string[] subjects);

        void CheckUniqueUserName(string userName, int? excludeId = null);

        void CheckExistByUserName(string userName);

        void CheckExistByEmail(string email);

        void CheckUniqueEmail(string email, int? excludeId = null);

        void CheckUniquePhone(string phone, int? excludeId = null);

        void CheckExistByPhone(string phone);

        #endregion
    }
}