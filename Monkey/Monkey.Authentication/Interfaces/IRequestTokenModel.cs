#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IRequestTokenModel.cs </Name>
//         <Created> 18/09/17 12:12:44 AM </Created>
//         <Key> ecf1f238-48e2-49ca-b824-18b8ad0f9029 </Key>
//     </File>
//     <Summary>
//         IRequestTokenModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Authentication.Constants;
using Monkey.Core.Constants;

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