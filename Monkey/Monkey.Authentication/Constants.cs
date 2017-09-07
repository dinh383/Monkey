#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> Constants.cs </Name>
//         <Created> 03/09/17 1:35:14 PM </Created>
//         <Key> a74f3b14-277a-4029-84f6-c90e1733cdba </Key>
//     </File>
//     <Summary>
//         Constants.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Authentication
{
    public static class Constants
    {
        public const string DefaultConfigSection = "Authentication";

        public static class TokenType
        {
            public const string Bearer = "Bearer";
            
            // Authorization Code
            public const string Code = "Code";
        }
    }
}