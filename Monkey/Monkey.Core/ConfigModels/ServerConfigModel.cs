#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ServerConfigModel.cs </Name>
//         <Created> 18/07/17 11:53:42 AM </Created>
//         <Key> 01bc1c56-5f44-486c-855a-32c070298040 </Key>
//     </File>
//     <Summary>
//         ServerConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.ConfigModels.Server;

namespace Monkey.Core.ConfigModels
{
    public class ServerConfigModel
    {
        public string AuthorName { get; set; }
        public string AuthorWebsite { get; set; }
        public string PoweredBy { get; set; }
        public string Name { get; set; }
        public CrosConfigModel CrosConfigModel { get; set; }
    }
}