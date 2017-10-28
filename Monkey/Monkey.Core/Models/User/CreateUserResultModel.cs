#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> CreateUserResultModel.cs </Name>
//         <Created> 28/10/2017 8:49:16 PM </Created>
//         <Key> 99e9d657-f812-4aa9-8e0c-dc25eda279c3 </Key>
//     </File>
//     <Summary>
//         CreateUserResultModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.Models.User
{
    public class CreateUserResultModel
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}