#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Facade </Project>
//     <File>
//         <Name> ClientService.cs </Name>
//         <Created> 02/10/17 3:53:10 PM </Created>
//         <Key> d38f2f43-5176-4f70-8650-8ba418b45ff1 </Key>
//     </File>
//     <Summary>
//         ClientService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Business.Auth;
using Monkey.Core.Models.Auth;
using Monkey.Service.Auth;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using Puppy.DependencyInjection.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Service.Facade.Auth
{
    [PerRequestDependency(ServiceType = typeof(IClientService))]
    public class ClientService : IClientService
    {
        private readonly IClientBusiness _clientBusiness;

        public ClientService(IClientBusiness clientBusiness)
        {
            _clientBusiness = clientBusiness;
        }

        public Task<int> CreateAsync(ClientCreateModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            _clientBusiness.CheckUniqueName(model.Name);
            return _clientBusiness.CreateAsync(model, cancellationToken);
        }

        public Task UpdateAsync(ClientUpdateModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            _clientBusiness.CheckExist(model.Id);
            _clientBusiness.CheckUniqueName(model.Name, model.Id);
            return _clientBusiness.UpdateAsync(model, cancellationToken);
        }

        public Task<ClientModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            _clientBusiness.CheckExist(id);
            return _clientBusiness.GetAsync(id, cancellationToken);
        }

        public Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _clientBusiness.GetDataTableAsync(model, cancellationToken);
        }

        public Task<string> GenerateSecretAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            _clientBusiness.CheckExist(id);
            return _clientBusiness.GenerateSecretAsync(id, cancellationToken);
        }

        public void CheckUniqueName(string name, int? excludeId)
        {
            _clientBusiness.CheckUniqueName(name, excludeId);
        }

        public Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            _clientBusiness.CheckExist(id);
            return _clientBusiness.RemoveAsync(id, cancellationToken);
        }
    }
}