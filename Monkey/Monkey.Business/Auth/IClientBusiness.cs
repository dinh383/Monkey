#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Interface </Project>
//     <File>
//         <Name> IClientBusiness.cs </Name>
//         <Created> 14/09/17 8:18:33 PM </Created>
//         <Key> cb340be9-ae66-469b-a702-bf2c7c754c8d </Key>
//     </File>
//     <Summary>
//         IClientBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models.Auth;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Business.Auth
{
    public interface IClientBusiness : IBaseBusiness
    {
        Task<int> CreateAsync(ClientCreateModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(ClientUpdateModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task<ClientModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

        Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GenerateSecretAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

        void CheckUniqueName(string name, int? excludeId = null);

        void CheckExist(params int[] ids);

        Task<int> GetIdAsync(string subject, string secret, CancellationToken cancellationToken = default(CancellationToken));

        void CheckExist(string subject, string secret);

        void CheckBanned(string subject, string secret);

        Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}