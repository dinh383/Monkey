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

using Monkey.Core.Configs.Models.MvcPath;
using Puppy.Core.StringUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monkey.Core.Configs.Models
{
    public class MvcPathConfigModel
    {
        public string WebRootFolderName { get; set; } = "wwwroot";

        public string AreasRootFolderName { get; set; } = "Areas";

        /// <summary>
        ///     Max Age in Cache Control Header 
        /// </summary>
        /// <remarks> Use the . separator between days and hours, see more: https://msdn.microsoft.com/en-us/library/system.timespan.aspx </remarks>
        public TimeSpan? MaxAgeResponseHeader { get; set; } = new TimeSpan(30, 0, 0, 0);

        public List<StaticsContentConfigModel> StaticsContents { get; set; }

        public List<string> GetStaticFoldersRelativePath()
        {
            List<string> listStaticFoldersRelativePath = new List<string>
            {
                WebRootFolderName
            };

            foreach (var staticContentConfig in StaticsContents)
            {
                var rootAreaRelativePath =
                    !string.IsNullOrWhiteSpace(AreasRootFolderName)
                        ? Path.Combine(AreasRootFolderName, staticContentConfig.Area, staticContentConfig.FolderRelativePath)
                        : Path.Combine(staticContentConfig.Area, staticContentConfig.FolderRelativePath);

                listStaticFoldersRelativePath.Add(rootAreaRelativePath);

                var rootAreaDirectoryInfo = new DirectoryInfo(rootAreaRelativePath.GetFullPath());

                var listSubFolder = rootAreaDirectoryInfo.GetDirectories("*", SearchOption.AllDirectories);

                var currentDirectory = Directory.GetCurrentDirectory();

                listStaticFoldersRelativePath.AddRange(listSubFolder.Select(subFolderInfo => subFolderInfo.FullName.Replace(currentDirectory, string.Empty).Trim(Path.DirectorySeparatorChar)));
            }

            listStaticFoldersRelativePath = listStaticFoldersRelativePath.Distinct().ToList();

            return listStaticFoldersRelativePath;
        }

        public List<string> GetStaticFilesRelativePath()
        {
            List<string> listStaticFilesRelativePath = new List<string>();

            List<string> listStaticFoldersRelativePath = GetStaticFoldersRelativePath();

            var currentDirectory = Directory.GetCurrentDirectory();

            foreach (var staticFolderRelativePath in listStaticFoldersRelativePath)
            {
                var staticFolderInfo = new DirectoryInfo(staticFolderRelativePath.GetFullPath());

                var listFileInfo = staticFolderInfo.GetFiles("*", SearchOption.AllDirectories);

                listStaticFilesRelativePath.AddRange(listFileInfo.Select(fileInfo => fileInfo.FullName.Replace(currentDirectory, string.Empty).Trim(Path.DirectorySeparatorChar)));
            }

            listStaticFilesRelativePath = listStaticFilesRelativePath.Distinct().ToList();

            return listStaticFilesRelativePath;
        }

        public List<string> GetStaticFilesRelativeUrl()
        {
            List<string> listStaticFoldersRelativeUrl = new List<string>();

            var webRootFolderAbsolutePath = WebRootFolderName.GetFullPath();

            var webRootFolderInfo = new DirectoryInfo(webRootFolderAbsolutePath);

            var listRootFileInfo = webRootFolderInfo.GetFiles("*", SearchOption.AllDirectories);

            listStaticFoldersRelativeUrl
                .AddRange(
                    listRootFileInfo
                        .Select(
                            fileInfo => fileInfo.FullName.Replace(webRootFolderAbsolutePath, string.Empty).Trim(Path.DirectorySeparatorChar)));

            foreach (var staticContentConfig in StaticsContents)
            {
                var rootHttpRequestPath = staticContentConfig.HttpRequestPath.Trim('\\', '/');

                var rootAreaRelativePath =
                    !string.IsNullOrWhiteSpace(AreasRootFolderName)
                        ? Path.Combine(AreasRootFolderName, staticContentConfig.Area, staticContentConfig.FolderRelativePath)
                        : Path.Combine(staticContentConfig.Area, staticContentConfig.FolderRelativePath);

                var rootAreaAbsolutePath = rootAreaRelativePath.GetFullPath();

                var rootAreaDirectoryInfo = new DirectoryInfo(rootAreaRelativePath.GetFullPath());

                var listFiles = rootAreaDirectoryInfo.GetFiles("*", SearchOption.AllDirectories);

                listStaticFoldersRelativeUrl
                    .AddRange(
                        listFiles
                            .Select(
                                fileInfo => rootHttpRequestPath
                                            + Path.DirectorySeparatorChar
                                            + fileInfo.FullName.Replace(rootAreaAbsolutePath, string.Empty).Trim(Path.DirectorySeparatorChar)));
            }

            listStaticFoldersRelativeUrl = listStaticFoldersRelativeUrl.Select(x => x.Replace(Path.DirectorySeparatorChar, '/').Trim('/')).Distinct().ToList();

            return listStaticFoldersRelativeUrl;
        }
    }
}