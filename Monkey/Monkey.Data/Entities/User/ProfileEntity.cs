#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> ProfileEntity.cs </Name>
//         <Created> 13/09/17 11:37:32 PM </Created>
//         <Key> b6dd6729-a547-4271-b3e0-00be4b89f32b </Key>
//     </File>
//     <Summary>
//         ProfileEntity.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Data.Entities.User
{
    public class ProfileEntity : Entity
    {
        public string FirstName { get; set; }

        public string FirstNameNorm { get; set; }

        public string LastName { get; set; }

        public string LastNameNorm { get; set; }

        public string FullName { get; set; }

        public string FullNameNorm { get; set; }

        public virtual UserEntity User { get; set; }
    }
}