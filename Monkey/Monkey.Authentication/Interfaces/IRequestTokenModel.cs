#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IRequestTokenModel.cs </Name>
//         <Created> 17/09/17 2:38:57 PM </Created>
//         <Key> e2b615f0-9806-40b6-8d4d-3171b7d07ae0 </Key>
//     </File>
//     <Summary>
//         IRequestTokenModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Authentication.Config;

namespace Monkey.Authentication.Interfaces
{
    public interface IRequestTokenModel
    {
        GrantType GrantType { get; set; }

        string ClientId { get; set; }

        string ClientSecret { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        string RefreshToken { get; set; }
    }
}