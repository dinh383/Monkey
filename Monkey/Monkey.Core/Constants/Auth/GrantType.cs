#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> GrantType.cs </Name>
//         <Created> 04/09/17 10:36:45 PM </Created>
//         <Key> 356817d4-f882-4f7e-af37-94c94bf1252d </Key>
//     </File>
//     <Summary>
//         GrantType.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.Constants.Auth
{
    public enum GrantType
    {
        Password = 1,
        RefreshToken = 2
    }
}