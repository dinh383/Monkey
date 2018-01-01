#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Facade </Project>
//     <File>
//         <Name> UserService.cs </Name>
//         <Created> 18/07/17 4:54:23 PM </Created>
//         <Key> 4e04746d-98a2-4f10-a6c6-b9a485ef0e44 </Key>
//     </File>
//     <Summary>
//         UserService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Business;
using Monkey.Business.Auth;
using Monkey.Business.User;
using Monkey.Core;
using Monkey.Core.Constants;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.User;
using Monkey.Service.User;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using Puppy.DependencyInjection.Attributes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Service.Facade.User
{
    [PerRequestDependency(ServiceType = typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IUserBusiness _userBusiness;

        private readonly IEmailBusiness _emailBusiness;

        private readonly IAuthenticationBusiness _authenticationBusiness;

        public UserService(IUserBusiness userBusiness, IEmailBusiness emailBusiness, IAuthenticationBusiness authenticationBusiness)
        {
            _userBusiness = userBusiness;
            _emailBusiness = emailBusiness;
            _authenticationBusiness = authenticationBusiness;
        }

        #region User - Get

        public Task<DataTableResponseDataModel<UserModel>> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default)
        {
            return _userBusiness.GetDataTableAsync(model, cancellationToken);
        }

        public Task<UserModel> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            _userBusiness.CheckExistsById(id);
            return _userBusiness.GetAsync(id, cancellationToken);
        }

        #endregion

        #region User - Create

        public async Task CreateByEmailAsync(CreateUserModel model, CancellationToken cancellationToken = default)
        {
            _userBusiness.CheckUniqueUserName(model.UserName);

            _userBusiness.CheckUniqueEmail(model.Email);

            _userBusiness.CheckUniquePhone(model.Phone);

            var createUserResult = await _userBusiness.CreateAsync(model, cancellationToken).ConfigureAwait(true);

            string activeToken = _authenticationBusiness.GenerateTokenConfirmEmail(createUserResult.Subject, model.Email, out var expireIn);

            _emailBusiness.SendActiveAccount(activeToken, model.Email, expireIn);
        }

        #endregion

        #region User - Update

        public Task UpdateAsync(UpdateUserModel model, CancellationToken cancellationToken = default)
        {
            _userBusiness.CheckExistsById(model.Id);

            if (model.Id == LoggedInUser.Current?.Id)
            {
                throw new MonkeyException(ErrorCode.UserSelfUpdate);
            }

            // Can't update last user have admin permission
            var listCannotUpdateUserId = _userBusiness.GetListLookupByPermissions(Enums.Permission.Admin).Select(x => x.Id).ToList();

            if (listCannotUpdateUserId.Count == 1 && listCannotUpdateUserId.FirstOrDefault() == model.Id)
            {
                throw new MonkeyException(ErrorCode.UserCannotUpdate);
            }

            _userBusiness.CheckUniqueUserName(model.UserName, model.Id);

            _userBusiness.CheckUniqueEmail(model.Email, model.Id);

            _userBusiness.CheckUniquePhone(model.Phone, model.Id);

            return _userBusiness.UpdateAsync(model, cancellationToken);
        }

        public async Task UpdateProfileAsync(UpdateProfileModel model, CancellationToken cancellationToken = default)
        {
            await _userBusiness.UpdateProfileAsync(model, cancellationToken).ConfigureAwait(true);

            // Update Logged In User
            LoggedInUser.Current = await _authenticationBusiness.GetLoggedInUserBySubjectAsync(LoggedInUser.Current.Subject, cancellationToken).ConfigureAwait(true);
        }

        #endregion

        #region User - Remove

        public Task RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            _userBusiness.CheckExistsById(id);

            // Can't remove theme self
            if (id == LoggedInUser.Current?.Id)
            {
                throw new MonkeyException(ErrorCode.UserSelfRemove);
            }

            // Can't remove last user have admin permission
            var listCannotRemoveUserId = _userBusiness.GetListLookupByPermissions(Enums.Permission.Admin).Select(x => x.Id).ToList();

            if (listCannotRemoveUserId.Count == 1 && listCannotRemoveUserId.FirstOrDefault() == id)
            {
                throw new MonkeyException(ErrorCode.UserCannotRemove);
            }

            return _userBusiness.RemoveAsync(id, cancellationToken);
        }

        #endregion

        #region User - Validation

        public void CheckUniqueUserName(string userName, int? excludeId)
        {
            _userBusiness.CheckUniqueUserName(userName, excludeId);
        }

        public void CheckUniqueEmail(string email, int? excludeId)
        {
            _userBusiness.CheckUniqueEmail(email, excludeId);
        }

        public void CheckUniquePhone(string phone, int? excludeId)
        {
            _userBusiness.CheckUniqueEmail(phone, excludeId);
        }

        public void CheckExistEmail(string email)
        {
            _userBusiness.CheckExistByEmail(email);
        }

        #endregion
    }
}