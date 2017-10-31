#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> Utils.cs </Name>
//         <Created> 11/10/17 11:47:54 AM </Created>
//         <Key> 57c307de-eb35-46c1-a2e6-62bd87283ba7 </Key>
//     </File>
//     <Summary>
//         Utils.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Configs;
using System;
using System.IO;

namespace Monkey.Core
{
    public class SystemUtils
    {
        public static string GetWebPhysicalPath(string path)
        {
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var pathUri))
            {
                throw new ArgumentException($"Invalid path {path}");
            }

            if (pathUri.IsAbsoluteUri) return path;

            path = Path.Combine(SystemConfig.MvcPath.WebRootFolderName, path);

            return path;
        }

        public static string GetWebUrl(string path)
        {
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var pathUri))
            {
                throw new ArgumentException($"Invalid path {path}");
            }

            if (pathUri.IsAbsoluteUri) return path;

            path = path.Replace(SystemConfig.MvcPath.WebRootFolderName, string.Empty).TrimStart('/').TrimStart('/').TrimStart('\\').TrimStart('\\');

            return path;
        }
    }
}