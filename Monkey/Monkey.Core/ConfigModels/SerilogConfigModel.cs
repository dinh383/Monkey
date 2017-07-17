#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> SerilogConfigModel.cs </Name>
//         <Created> 17/07/17 8:47:09 PM </Created>
//         <Key> ce9000f9-30cf-47fe-9203-18f1ac937955 </Key>
//     </File>
//     <Summary>
//         SerilogConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System.Collections.Generic;
using Monkey.Core.ConfigModels.Serilog;

namespace Monkey.Core.ConfigModels
{
    public class SerilogConfigModel
    {
        public MinimumlevelConfigModel MinimumLevel { get; set; }
        public List<string> Using { get; set; }
        public List<string> Enrich { get; set; }
        public List<WriteToConfigModel> WriteTo { get; set; }
    }
}