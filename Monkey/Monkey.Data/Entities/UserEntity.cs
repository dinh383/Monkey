#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> UserEntity.cs </Name>
//         <Created> 18/07/17 4:11:13 PM </Created>
//         <Key> abe1ae76-0722-4dba-8ebf-51eb28f5796a </Key>
//     </File>
//     <Summary>
//         UserEntity.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Data.Entities
{
    public class UserEntity : Entity
    {
        public string UserName { get; set; }

        public string UserNameNorm { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
    }
}