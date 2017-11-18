#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> dung.nguyen </Author>
//     <Project> Monkey → Service Interface </Project>
//     <File>
//         <Name> IImageService.cs </Name>
//         <Created> 10/19/2017 02:29:38 PM </Created>
//         <Key> 256b3766-e05f-446b-b062-983dbd7fbf2e </Key>
//     </File>
//     <Summary>
//         IImageService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Service
{
    public interface IImageService : IBaseService
    {
        Task<int> CreateAsync(AddImageModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(UpdateImageModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task<ImageModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

        Task<DataTableResponseDataModel<ImageModel>> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}