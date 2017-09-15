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

using Monkey.Core.Models.User;

namespace Monkey.Core
{
    public static class LoggedInUser
    {
        public static LoggedUserModel Current { get; set; }
    }
}