﻿#region	License
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
        private readonly IRoleBusiness _roleBusiness;
        private readonly IUserBusiness _userBusiness;
        private int _roleAdminId = 1;

        public SeedDataService(IRoleBusiness roleBusiness, IUserBusiness userBusiness)
        {
            _roleBusiness = roleBusiness;
            _userBusiness = userBusiness;
        }

        public void SeedData()
        {
            InitialRoleAsync().Wait();
            InitialUserAsync().Wait();
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
                }
                catch
                {
                    // If already have user => Ignore
                }
                var roleId = await _roleBusiness.CreateAsync(roleName, roleDescription, enumMember.Value).ConfigureAwait(true);

                if (enumMember.Value == Enums.Permission.Admin)
                {
                    _roleAdminId = roleId;
                }
            }
        }

        public async Task InitialUserAsync()
        {

            string userName = "topnguyen";
            string email = "topnguyen92@gmail.com";
            string password = "Password123@@";
            try
            {
                _userBusiness.CheckUniqueUserName(userName);
                _userBusiness.CheckUniqueEmail(email);
            }
            catch
            {
                // If already have user => Ignore
            }
            var subject = await _userBusiness.CreateUserByEmailAsync(email, _roleAdminId).ConfigureAwait(true);
            await _userBusiness.ActiveByEmailAsync(subject, userName, password).ConfigureAwait(true);
        }
    }
}