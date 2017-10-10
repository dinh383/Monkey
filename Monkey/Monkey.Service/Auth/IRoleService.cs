#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Interface </Project>
//     <File>
//         <Name> IRoleService.cs </Name>
//         <Created> 06/10/17 10:55:25 PM </Created>
//         <Key> 9fcf0524-20f6-413e-a1cf-1e560b84a0da </Key>
//     </File>
//     <Summary>
//         IRoleService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models;
using Monkey.Core.Models.Auth;
using Puppy.Web.Models.Api;
using System.Threading.Tasks;

namespace Monkey.Service.Auth
{
    public interface IRoleService : IBaseService
    {
        Task<PagedCollectionResultModel<RoleModel>> GetListRoleAsync(PagedCollectionParametersModel model);
    }
}