#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Interface </Project>
//     <File>
//         <Name> IClientService.cs </Name>
//         <Created> 02/10/17 3:48:36 PM </Created>
//         <Key> 4b604e36-7c6f-489a-9242-c9e466d595ec </Key>
//     </File>
//     <Summary>
//         IClientService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models.Auth;
using System.Threading.Tasks;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;

namespace Monkey.Service
{
    public interface IClientService : IBaseService
    {
        Task<int> CreateAsync(ClientCreateModel model);

        Task UpdateAsync(ClientUpdateModel model);

        Task<ClientModel> GetAsync(int id);

        Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model);

        Task<string> GenerateSecretAsync(int id);

        void CheckUniqueName(string name, int? excludeId);
    }
}