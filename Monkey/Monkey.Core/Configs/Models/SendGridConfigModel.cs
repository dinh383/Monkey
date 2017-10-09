#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> SendGridConfigModel.cs </Name>
//         <Created> 09/10/17 11:06:54 PM </Created>
//         <Key> 3db1ca01-d05a-4ba6-ab89-df0c5675d04c </Key>
//     </File>
//     <Summary>
//         SendGridConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.Configs.Models
{
    public class SendGridConfigModel
    {
        public string Key { get; set; }

        public string SenderDisplayEmail { get; set; }

        public string SenderDisplayName { get; set; }
    }
}