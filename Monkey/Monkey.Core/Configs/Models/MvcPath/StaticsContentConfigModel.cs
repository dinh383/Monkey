#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> StaticsContentConfigModel.cs </Name>
//         <Created> 23/07/17 12:06:02 PM </Created>
//         <Key> c1b5530b-fd14-4561-8236-7271179489ce </Key>
//     </File>
//     <Summary>
//         StaticsContentConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;

namespace Monkey.Core.Configs.Models.MvcPath
{
    public class StaticsContentConfigModel
    {
        /// <summary>
        ///     Area 
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        ///     Use exactly folder name case in explorer 
        /// </summary>
        /// <remarks> Relative path from <see cref="Area" /> </remarks>
        public string FolderRelativePath { get; set; }

        /// <summary>
        ///     Use lower case for http request 
        /// </summary>
        public string HttpRequestPath { get; set; }

        /// <summary>
        ///     Max Age in Cache Control Header 
        /// </summary>
        /// <remarks> Use the . separator between days and hours, see more: https://msdn.microsoft.com/en-us/library/system.timespan.aspx </remarks>
        public TimeSpan? MaxAgeResponseHeader { get; set; }
    }
}