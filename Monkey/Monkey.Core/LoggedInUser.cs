#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> LoggedInUser.cs </Name>
//         <Created> 15/09/17 2:28:02 PM </Created>
//         <Key> 44f1e3fb-31f9-4c9c-8daf-2b65daf4e86c </Key>
//     </File>
//     <Summary>
//         LoggedInUser.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models.Auth;
using Puppy.Core.DictionaryUtils;
using Puppy.Core.ObjectUtils;
using Puppy.Core.TypeUtils;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monkey.Core
{
    public static class LoggedInUser
    {
        private static string HttpContextItemKey => typeof(LoggedInUser).GetAssembly().FullName;

        public static LoggedInUserModel Current
        {
            get
            {
                if (HttpContext.Current?.Items != null)
                {
                    return HttpContext.Current.Items.TryGetValue(HttpContextItemKey, out var value)
                        ? value?.ConvertTo<LoggedInUserModel>()
                        : null;
                }

                return null;
            }
            set
            {
                // Update Current Logged In User in both Static Global variable and HttpContext
                if (HttpContext.Current.Items?.Any() != null)
                {
                    HttpContext.Current.Items = new Dictionary<object, object>();
                }

                HttpContext.Current.Items.AddOrUpdate(HttpContextItemKey, value);
            }
        }
    }
}