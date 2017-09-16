#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> IdentityModel.cs </Name>
//         <Created> 16/09/17 2:14:41 PM </Created>
//         <Key> c7f89c43-8d35-4244-b0c8-10257a91ce2b </Key>
//     </File>
//     <Summary>
//         IdentityModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.Models.Config
{
    public class IdentityModel
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}