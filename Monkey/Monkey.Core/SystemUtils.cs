﻿#region	License
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
using Puppy.Core.DateTimeUtils;
using System;
using System.IO;

namespace Monkey.Core
{
    public static class SystemUtils
    {
        #region Date Time

        public static readonly TimeZoneInfo SystemTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(SystemConfig.SystemTimeZone);

        public static DateTime SystemTimeNow => DateTimeOffset.UtcNow.UtcToSystemTime();

        /// <summary>
        ///     Null or less than system now will use system now value 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimeOffset GetAtLeastSystemTimeNow(DateTime? dateTime)
        {
            var systemNow = SystemTimeNow;

            dateTime = dateTime ?? SystemTimeNow;

            var dateTimeAtLeastSystemNow = dateTime < systemNow ? systemNow : dateTime.Value;

            return dateTimeAtLeastSystemNow;
        }

        /// <summary>
        ///     Null or less than system now will use system now value 
        /// </summary>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        public static DateTimeOffset GetAtLeastSystemTimeNow(DateTimeOffset? dateTimeOffset)
        {
            var systemNow = SystemTimeNow;

            dateTimeOffset = dateTimeOffset ?? SystemTimeNow;

            var dateTimeAtLeastSystemNow = dateTimeOffset < systemNow ? systemNow : dateTimeOffset.Value;

            return dateTimeAtLeastSystemNow;
        }

        public static DateTime UtcToSystemTime(this DateTimeOffset dateTimeOffsetUtc)
        {
            return dateTimeOffsetUtc.UtcDateTime.GetDateTimeFromUtc(SystemTimeZoneInfo);
        }

        public static DateTime UtcToSystemTime(this DateTime dateTimeUtc)
        {
            return dateTimeUtc.GetDateTimeFromUtc(SystemTimeZoneInfo);
        }

        #endregion

        #region Path

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

        #endregion
    }
}