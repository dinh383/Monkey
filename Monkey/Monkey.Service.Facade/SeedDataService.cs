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

using EnumsNET;
using Monkey.Business.Auth;
using Puppy.DependencyInjection.Attributes;
using System.Threading.Tasks;
using Enums = Monkey.Core.Constants.Enums;

namespace Monkey.Service.Facade
{
    [PerRequestDependency(ServiceType = typeof(ISeedDataService))]
    public class SeedDataService : ISeedDataService
    {
        private readonly IAuthenticationBusiness _authenticationBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private int _roleAdminId = 1;

        public SeedDataService(IAuthenticationBusiness authenticationBusiness, IRoleBusiness roleBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
            _roleBusiness = roleBusiness;
        }

        public void SeedData()
        {
            InitialRoleAsync().Wait();
            InitialUserAsync().Wait();
        }

        public async Task InitialRoleAsync()
        {
            try
            {
                string roleName = Enums.Permission.Admin.ToString();
                string roleDescription = Enums.Permission.Admin.AsString(EnumFormat.Description);

                _roleBusiness.CheckUniqueName(roleName);

                _roleAdminId = await _roleBusiness.CreateAsync(roleName, roleDescription, Enums.Permission.Admin).ConfigureAwait(true);
            }
            catch
            {
                // If already have user => Ignore
            }
        }

        public async Task InitialUserAsync()
        {
            try
            {
                string userName = "topnguyen";
                string email = "topnguyen92@gmail.com";
                string password = "Password123@@";

                _authenticationBusiness.CheckUniqueUserName(userName);

                _authenticationBusiness.CheckUniqueEmail(email);

                var subject = await _authenticationBusiness.CreateUserByEmailAsync(email, _roleAdminId).ConfigureAwait(true);

                await _authenticationBusiness.ActiveByEmailAsync(subject, userName, password).ConfigureAwait(true);
            }
            catch
            {
                // If already have user => Ignore
            }
        }
    }
}