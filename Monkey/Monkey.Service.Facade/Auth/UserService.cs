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

using Monkey.Business.Auth;
using Monkey.Core.Models.Auth;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using Puppy.DependencyInjection.Attributes;
using System.Threading.Tasks;

namespace Monkey.Service.Facade.Auth
{
    [PerRequestDependency(ServiceType = typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IUserBusiness _userBusiness;

        public UserService(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        public Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model)
        {
            return _userBusiness.GetDataTableAsync(model);
        }

        public Task CreateAsync(UserCreateModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                _userBusiness.CheckUniqueEmail(model.Email);
            }

            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                _userBusiness.CheckUniquePhone(model.Phone);
            }

            return _userBusiness.CreateUserByEmailAsync(model.Email, model.RoleId);
        }

        public Task<UserModel> GetAsync(int id)
        {
            _userBusiness.CheckExistsById(id);
            return _userBusiness.GetAsync(id);
        }

        public Task UpdateAsync(UserUpdateModel model)
        {
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
            return _userBusiness.RemoveAsync(id);
        }

        public void CheckUniqueUserName(string userName, int? excludeId)
        {
            _userBusiness.CheckUniqueUserName(userName, excludeId);
        }
    }
}