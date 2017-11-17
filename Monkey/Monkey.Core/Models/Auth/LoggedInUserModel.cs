#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> LoggedUserModel.cs </Name>
//         <Created> 13/09/17 10:33:42 PM </Created>
//         <Key> 45c835b1-4933-4f5e-8d6e-a89477e16a00 </Key>
//     </File>
//     <Summary>
//         LoggedUserModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Constants;
using Monkey.Core.Models.User;
using System.Collections.Generic;
using System.Linq;

namespace Monkey.Core.Models.Auth
{
    public class LoggedInUserModel : UserModel
    {
        public int? ClientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Enums.Permission> ListPermission { get; set; }

        public string AccessToken { get; set; }

        public string AccessTokenType { get; set; }

        /// <summary>
        ///     Check current logged in user have any permission in <see cref="permissions" /> 
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public bool IsHaveAnyPermissions(params Enums.Permission[] permissions)
        {
            if (permissions?.Any() != true)
            {
                return true;
            }

            if (ListPermission?.Any() != true)
            {
                return false;
            }

            return permissions.Any(ListPermission.Contains);
        }

        /// <summary>
        ///     Check current logged in user have all permission in <see cref="permissions" /> 
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public bool IsHaveAllPermissions(params Enums.Permission[] permissions)
        {
            if (permissions?.Any() != true)
            {
                return true;
            }

            return ListPermission?.Any() == true && permissions.All(ListPermission.Contains);
        }

        /// <summary>
        ///     Check current logged in user have any permission in <see cref="permissions" /> 
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public bool IsHaveAnyPermissions(IEnumerable<Enums.Permission> permissions)
        {
            var listPermission = permissions?.ToList();

            if (listPermission?.Any() != true)
            {
                return true;
            }

            return ListPermission?.Any() == true && listPermission.Any(ListPermission.Contains);
        }

        /// <summary>
        ///     Check current logged in user have all permission in <see cref="permissions" /> 
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public bool IsHaveAllPermissions(IEnumerable<Enums.Permission> permissions)
        {
            var listPermission = permissions?.ToList();

            if (listPermission?.Any() != true)
            {
                return true;
            }

            return ListPermission?.Any() == true && listPermission.All(ListPermission.Contains);
        }
    }
}