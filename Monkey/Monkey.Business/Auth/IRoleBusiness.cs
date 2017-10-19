#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Interface </Project>
//     <File>
//         <Name> IRoleBusiness.cs </Name>
//         <Created> 06/10/17 10:56:07 PM </Created>
//         <Key> 4cb74977-03c8-400e-b357-6a3e9df12654 </Key>
//     </File>
//     <Summary>
//         IRoleBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Constants;
using Monkey.Core.Models;
using Monkey.Core.Models.Auth;
using Puppy.Web.Models.Api;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Business.Auth
{
    public interface IRoleBusiness : IBaseBusiness
    {
        void CheckUniqueName(string name, int? excludeId = null);

        Task<int> CreateAsync(string name, string description, CancellationToken cancellationToken = default(CancellationToken), params Enums.Permission[] permissions);

        Task<PagedCollectionResultModel<RoleModel>> GetListRoleAsync(PagedCollectionParametersModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task<RoleModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}