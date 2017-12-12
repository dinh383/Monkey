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
        #region Get

        Task<DataTableResponseDataModel<ClientModel>> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default);

        Task<ClientModel> GetAsync(int id, CancellationToken cancellationToken = default);

        Task<int> GetIdAsync(string subject, string secret, CancellationToken cancellationToken = default);

        #endregion

        #region Create

        Task<int> CreateAsync(ClientCreateModel model, CancellationToken cancellationToken = default);

        #endregion

        #region Update

        Task UpdateAsync(ClientUpdateModel model, CancellationToken cancellationToken = default);

        Task<string> GenerateSecretAsync(int id, CancellationToken cancellationToken = default);

        #endregion

        #region Remove

        Task RemoveAsync(int id, CancellationToken cancellationToken = default);

        #endregion

        #region Validation

        void CheckUniqueName(string name, int? excludeId = null);

        void CheckExist(params int[] ids);

        void CheckExist(string subject, string secret);

        void CheckBanned(string subject, string secret);

        #endregion
    }
}