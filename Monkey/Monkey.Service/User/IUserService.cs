﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Interface </Project>
//     <File>
//         <Name> IUserService.cs </Name>
//         <Created> 18/07/17 4:50:08 PM </Created>
//         <Key> fb882904-c8f5-4268-92b9-2f0bb347372a </Key>
//     </File>
//     <Summary>
//         IUserService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models.User;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Service.User
{
    public interface IUserService
    {
        Task<DataTableResponseDataModel<UserModel>> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task CreateByEmailAsync(UserCreateModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task<UserModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(UserUpdateModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

        void CheckUniqueUserName(string userName, int? excludeId);

        void CheckUniqueEmail(string email, int? excludeId);

        void CheckUniquePhone(string phone, int? excludeId);

        void CheckExistEmail(string email);

        Task UpdateProfileAsync(UpdateProfileModel model, CancellationToken cancellationToken = default(CancellationToken));
    }
}