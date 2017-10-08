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

using System.Threading.Tasks;
using Monkey.Business.Auth;
using Monkey.Core.Models.Auth;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using Puppy.DependencyInjection.Attributes;

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
            throw new System.NotImplementedException();
        }

        public Task CreateAsync(UserCreateModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserModel> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(UserUpdateModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public void CheckUniqueUserName(string name, int? excludeId)
        {
            throw new System.NotImplementedException();
        }
    }
}