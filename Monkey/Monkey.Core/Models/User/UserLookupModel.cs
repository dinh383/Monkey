#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> UserLookupModel.cs </Name>
//         <Created> 10/12/2017 2:47:32 PM </Created>
//         <Key> 47ad6282-d0c9-49a4-b1d9-bc7d454221e4 </Key>
//     </File>
//     <Summary>
//         UserLookupModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.Models.User
{
    public class UserLookupModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string FullName { get; set; }
    }
}