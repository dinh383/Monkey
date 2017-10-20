#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> dung.nguyen </Author>
//     <Project> Monkey → Business Interface </Project>
//     <File>
//         <Name> IImageBusiness.cs </Name>
//         <Created> 10/19/2017 02:26:56 PM </Created>
//         <Key> aab661a8-a9af-4d39-97a7-a399fcc66491 </Key>
//     </File>
//     <Summary>
//         IImageBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Monkey.Core.Models;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;

namespace Monkey.Business
{
    public interface IImageBusiness : IBaseBusiness
    {
        Task<int> CreateAsync(ImageAddModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(ImageAddModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task<ImageModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

        Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default(CancellationToken));
        void CheckExist(params int[] ids);
        Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}