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

using Microsoft.EntityFrameworkCore;
using Monkey.Core.Entities.Auth;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Data.Auth;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;
using Puppy.DependencyInjection.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Business.Logic
{
    [PerRequestDependency(ServiceType = typeof(IClientBusiness))]
    public class ClientBusiness : IClientBusiness
    {
        private readonly IClientRepository _clientRepository;

        public ClientBusiness(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Task<int> GetTotalAsync()
        {
            return _clientRepository.Get().CountAsync();
        }

        public Task<ClientModel> CreateAsync(ClientCreateModel model)
        {
            var clientEntity = model.MapTo<ClientEntity>();

            _clientRepository.Add(clientEntity);

            _clientRepository.SaveChanges();

            var clientModel = clientEntity.MapTo<ClientModel>();

            return Task.FromResult(clientModel);
        }

        public async Task<int> GetIdAsync(string globalId, string secret)
        {
            var clientId = await _clientRepository.Get(x => x.GlobalId == globalId && x.Secret == secret).Select(x => x.Id).SingleAsync().ConfigureAwait(true);
            return clientId;
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

        public void CheckExistByName(params string[] names)
        {
            names = names.Distinct().Select(StringHelper.Normalize).ToArray();
            int totalInDb = _clientRepository.Get(x => names.Contains(x.NameNorm)).Count();
            if (totalInDb != names.Length)
            {
                throw new MonkeyException(ErrorCode.InvalidClient);
            }
        }
    }
}