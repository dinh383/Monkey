#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Logic </Project>
//     <File>
//         <Name> ClientBusiness.cs </Name>
//         <Created> 14/09/17 8:20:36 PM </Created>
//         <Key> 00b303c0-eb62-4e2f-8011-8ca85ae75e0c </Key>
//     </File>
//     <Summary>
//         ClientBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Business.Auth;
using Monkey.Core.Entities.Auth;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Data.Auth;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;
using Puppy.DataTable;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Business.Logic.Auth
{
    [PerRequestDependency(ServiceType = typeof(IClientBusiness))]
    public class ClientBusiness : IClientBusiness
    {
        private readonly IClientRepository _clientRepository;

        public ClientBusiness(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Task<int> CreateAsync(ClientCreateModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            var clientEntity = model.MapTo<ClientEntity>();

            _clientRepository.Add(clientEntity);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _clientRepository.SaveChanges();

            return Task.FromResult(clientEntity.Id);
        }

        public Task UpdateAsync(ClientUpdateModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            var clientEntity = model.MapTo<ClientEntity>();

            clientEntity.BannedTime = model.IsBanned ? DateTimeOffset.UtcNow : (DateTimeOffset?)null;

            clientEntity.BannedRemark = model.IsBanned ? null : clientEntity.BannedRemark;

            _clientRepository.Update(clientEntity, x => x.Name, x => x.Domains, x => x.Type, x => x.BannedTime, x => x.BannedRemark);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _clientRepository.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<ClientModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var model = _clientRepository.Get(x => x.Id == id).QueryTo<ClientModel>().FirstOrDefault();
            return Task.FromResult(model);
        }

        public Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            var listData = _clientRepository.Get().QueryTo<ClientModel>();

            var result = listData.GetDataTableResponse(model);

            return Task.FromResult(result);
        }

        public Task<string> GenerateSecretAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            ClientEntity client = new ClientEntity
            {
                Id = id,
                GlobalId = Guid.NewGuid().ToString("N")
            };

            _clientRepository.Update(client, x => x.GlobalId);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _clientRepository.SaveChanges();

            return Task.FromResult(client.GlobalId);
        }

        public void CheckExist(params int[] ids)
        {
            ids = ids.Distinct().ToArray();
            int totalInDb = _clientRepository.Get(x => ids.Contains(x.Id)).Count();
            if (totalInDb != ids.Length)
            {
                throw new MonkeyException(ErrorCode.NotFound);
            }
        }

        public Task<int> GetIdAsync(string subject, string secret, CancellationToken cancellationToken = default(CancellationToken))
        {
            var clientId = _clientRepository.Get(x => x.GlobalId == subject && x.Secret == secret).Select(x => x.Id).Single();
            return Task.FromResult(clientId);
        }

        public void CheckExist(string subject, string secret)
        {
            bool isExist = _clientRepository.Get(x => x.GlobalId == subject && x.Secret == secret).Any();
            if (!isExist)
            {
                throw new MonkeyException(ErrorCode.InvalidClient);
            }
        }

        public void CheckBanned(string subject, string secret)
        {
            var clientShortInfo = _clientRepository.Get(x => x.GlobalId == subject && x.Secret == secret).Select(x => new
            {
                x.Id,
                x.BannedTime,
                x.BannedRemark
            }).Single();

            if (clientShortInfo.BannedTime != null)
            {
                throw new MonkeyException(ErrorCode.ClientIsBanned, clientShortInfo.BannedRemark);
            }
        }

        public Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            _clientRepository.Delete(new ClientEntity
            {
                Id = id
            });

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _clientRepository.SaveChanges();

            return Task.CompletedTask;
        }

        public void CheckUniqueName(string name, int? excludeId = null)
        {
            string nameNorm = StringHelper.Normalize(name);

            var query = _clientRepository.Get(x => x.NameNorm == nameNorm);

            if (excludeId != null)
            {
                query = query.Where(x => x.Id != excludeId);
            }

            if (query.Any())
            {
                throw new MonkeyException(ErrorCode.ClientNameNotUnique);
            }
        }
    }
}