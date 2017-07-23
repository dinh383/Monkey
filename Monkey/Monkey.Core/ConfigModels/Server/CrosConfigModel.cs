#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> CrosConfigModel.cs </Name>
//         <Created> 18/07/17 11:59:55 AM </Created>
//         <Key> 6fa8500b-a4b1-41da-adee-d2af676d3206 </Key>
//     </File>
//     <Summary>
//         CrosConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.ConfigModels.Server
{
    public class CrosConfigModel
    {
        public string PolicyAllowAllName { get; set; }
        public string AccessControlAllowOrigin { get; set; }
        public string AccessControlAllowHeaders { get; set; }
        public string AccessControlAllowMethods { get; set; }
    }
}