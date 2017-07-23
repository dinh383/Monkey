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
using System;

namespace Monkey.Core.ConfigModels
{
    public class ServerConfigModel
    {
        /// <summary>
        ///     Author Full Name 
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        ///     Author of Website URL 
        /// </summary>
        public string AuthorWebsite { get; set; }

        /// <summary>
        ///     Author Email 
        /// </summary>
        public string AuthorEmail { get; set; }

        /// <summary>
        ///     Power By Technology 
        /// </summary>
        public string PoweredBy { get; set; }

        /// <summary>
        ///     Server Name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     System Time Zone Info, Can Find Full list ID via Current Machine System 
        /// </summary>
        /// <remarks>
        ///     System store list Time Zone Info in <c> Regedit Key </c>:
        ///     "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones"
        /// </remarks>
        public string TimeZoneId { get; set; }

        public TimeZoneInfo TimeZoneInfo => TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);

        /// <summary>
        ///     Cookie Schema Name should unique in server machine between web applications 
        /// </summary>
        public string CookieSchemaName { get; set; }

        /// <summary>
        ///     Cros Policy 
        /// </summary>
        public CrosConfigModel Cros { get; set; }
    }
}