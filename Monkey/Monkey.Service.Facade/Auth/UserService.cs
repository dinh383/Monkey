﻿#region	License
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
using Monkey.Core;
using Monkey.Core.Constants;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Service.Auth;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Service.Facade.Auth
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

        public Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model)
        {
            return _userBusiness.GetDataTableAsync(model);
        }

        public async Task CreateByEmailAsync(UserCreateModel model)
        {
            _userBusiness.CheckUniqueEmail(model.Email);

            string subject = await _userBusiness.CreateUserByEmailAsync(model.Email, model.RoleId).ConfigureAwait(true);

            string activeToken = _authenticationBusiness.GenerateTokenConfirmEmail(subject, model.Email, out TimeSpan expireIn);

            _emailBusiness.SendActiveAccount(activeToken, model.Email, expireIn);
        }

        public Task<UserModel> GetAsync(int id)
        {
            _userBusiness.CheckExistsById(id);
            return _userBusiness.GetAsync(id);
        }

        public Task UpdateAsync(UserUpdateModel model)
        {
            _userBusiness.CheckExistsById(model.Id);

            if (model.Id == LoggedInUser.Current?.Id)
            {
                throw new MonkeyException(ErrorCode.UserSelfUpdate);
            }

            // Can't update last user have permission
            var listCannotUpdateUserId = _userBusiness.ListUserIdByPermissions(Enums.Permission.Admin);
            if (listCannotUpdateUserId.Count == 1 && listCannotUpdateUserId.FirstOrDefault() == model.Id)
            {
                throw new MonkeyException(ErrorCode.UserCannotUpdate);
            }

            _userBusiness.CheckUniqueUserName(model.UserName, model.Id);

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                _userBusiness.CheckUniqueEmail(model.Email, model.Id);
            }

            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                _userBusiness.CheckUniquePhone(model.Phone, model.Id);
            }

            return _userBusiness.UpdateAsync(model);
        }

        public Task RemoveAsync(int id)
        {
            _userBusiness.CheckExistsById(id);

            // Can't remove theme self
            if (id == LoggedInUser.Current?.Id)
            {
                throw new MonkeyException(ErrorCode.UserSelfRemove);
            }

            // Can't remove last user have permission
            var listCannotRemoveUserId = _userBusiness.ListUserIdByPermissions(Enums.Permission.Admin);
            if (listCannotRemoveUserId.Count == 1 && listCannotRemoveUserId.FirstOrDefault() == id)
            {
                throw new MonkeyException(ErrorCode.UserCannotRemove);
            }

            return _userBusiness.RemoveAsync(id);
        }

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
    }
}