#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> MvcPathConfigModel.cs </Name>
//         <Created> 23/07/17 11:21:00 AM </Created>
//         <Key> c80bc062-f496-4fce-8c1d-cd3e6548201f </Key>
//     </File>
//     <Summary>
//         MvcPathConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;
using Monkey.Core.ConfigModels.MvcPath;
using System.Collections.Generic;

namespace Monkey.Core.ConfigModels
{
    public class MvcPathConfigModel
    {
        public string WebRootFolderName { get; set; } = "wwwroot";

        public string AreasRootFolderName { get; set; } = "Areas";

        /// <summary>
        /// Max Age in Cache Control Header
        /// </summary>
        /// <remarks>Use the . separator between days and hours, see more: https://msdn.microsoft.com/en-us/library/system.timespan.aspx </remarks>
        public TimeSpan? MaxAgeResponseHeader { get; set; }

        public List<StaticsContentConfigModel> StaticsContents { get; set; }
    }
}