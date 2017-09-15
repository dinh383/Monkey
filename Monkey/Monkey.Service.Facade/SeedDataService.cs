#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Facade </Project>
//     <File>
//         <Name> SeedDataService.cs </Name>
//         <Created> 14/09/17 11:21:24 AM </Created>
//         <Key> ffec5eb8-f942-4a6d-b157-55b7e11dec23 </Key>
//     </File>
//     <Summary>
//         SeedDataService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Business;
using Monkey.Core.Constants;
using Monkey.Core.Models.Client;
using Puppy.DependencyInjection.Attributes;
using System.Threading.Tasks;

namespace Monkey.Service.Facade
{
    [PerRequestDependency(ServiceType = typeof(ISeedDataService))]
    public class SeedDataService : ISeedDataService
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IAuthenticationBusiness _authenticationBusiness;
        private readonly IClientBusiness _clientBusiness;

        public SeedDataService(IUserBusiness userBusiness, IAuthenticationBusiness authenticationBusiness, IClientBusiness clientBusiness)
        {
            _userBusiness = userBusiness;
            _authenticationBusiness = authenticationBusiness;
            _clientBusiness = clientBusiness;
        }

        public void SeedData()
        {
            InitialUserAsync().Wait();
            InitialClientAsync().Wait();
        }

        public async Task InitialUserAsync()
        {
            if (await _userBusiness.GetTotalAsync().ConfigureAwait(true) <= 0)
            {
                var globalId = await _userBusiness.CreateAsync("topnguyen92@gmail.com").ConfigureAwait(true);
                string passwordHash = _authenticationBusiness.HashPassword("123456", out string salt);
                await _userBusiness.ActiveByEmailAsync(globalId, "topnguyen", passwordHash, salt).ConfigureAwait(true);
            }
        }

        public async Task InitialClientAsync()
        {
            if (await _clientBusiness.GetTotalAsync().ConfigureAwait(true) <= 0)
            {
                await _clientBusiness.CreateAsync(new ClientCreateModel
                {
                    Name = "Monkey",
                    Type = Enums.ClientType.Website
                }).ConfigureAwait(true);
            }
        }
    }
}