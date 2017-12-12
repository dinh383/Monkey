#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Facade </Project>
//     <File>
//         <Name> InitialService.cs </Name>
//         <Created> 14/09/17 11:21:24 AM </Created>
//         <Key> ffec5eb8-f942-4a6d-b157-55b7e11dec23 </Key>
//     </File>
//     <Summary>
//         InitialService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using EnumsNET;
using Monkey.Business;
using Monkey.Business.Auth;
using Monkey.Business.User;
using Monkey.Core.Models.User;
using Puppy.DependencyInjection.Attributes;
using System.Threading;
using System.Threading.Tasks;
using Enums = Monkey.Core.Constants.Enums;

namespace Monkey.Service.Facade
{
    [PerRequestDependency(ServiceType = typeof(IInitialService))]
    public class InitialService : IInitialService
    {
        private readonly IRoleBusiness _roleBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IAuthenticationBusiness _authenticationBusiness;
        private readonly IInitialBusiness _initialBusiness;
        private int _roleAdminId = 1;

        public InitialService(
            IRoleBusiness roleBusiness,
            IUserBusiness userBusiness,
            IAuthenticationBusiness authenticationBusiness,
            IInitialBusiness initialBusiness)
        {
            _roleBusiness = roleBusiness;
            _userBusiness = userBusiness;
            _authenticationBusiness = authenticationBusiness;
            _initialBusiness = initialBusiness;
        }

        public void InitialData()
        {
            InitialRoleAsync().Wait();
            InitialUserAsync().Wait();
        }

        public void ReBuildCache()
        {
            _initialBusiness.ReBuildConfigurationCache();
        }

        public async Task InitialRoleAsync()
        {
            foreach (var enumMember in EnumsNET.Enums.GetMembers<Enums.Permission>())
            {
                string roleName = enumMember.Value.AsString(EnumFormat.Name);
                string roleDescription = enumMember.Value.AsString(EnumFormat.Description);
                try
                {
                    _roleBusiness.CheckUniqueName(roleName);

                    var roleId = await _roleBusiness.CreateAsync(roleName, roleDescription, default(CancellationToken), enumMember.Value).ConfigureAwait(true);

                    if (enumMember.Value == Enums.Permission.Admin)
                    {
                        _roleAdminId = roleId;
                    }
                }
                catch
                {
                    // If already have user => Ignore
                }
            }
        }

        public async Task InitialUserAsync()
        {
            string userName = "topnguyen";
            string fullName = "Top Nguyễn";
            string email = "topnguyen92@gmail.com";
            string password = "Password123@@";
            try
            {
                _userBusiness.CheckUniqueUserName(userName);
                _userBusiness.CheckUniqueEmail(email);

                var createUserResult = await _userBusiness.CreateAsync(new UserCreateModel
                {
                    Email = email,
                    UserName = userName,
                    FullName = fullName,
                    RoleId = _roleAdminId
                }).ConfigureAwait(true);

                await _authenticationBusiness.ConfirmEmailAsync(createUserResult.Subject, userName, password).ConfigureAwait(true);
            }
            catch
            {
                // If already have user => Ignore
            }
        }
    }
}