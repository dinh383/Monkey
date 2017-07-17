#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> SystemUtils.cs </Name>
//         <Created> 19/05/2017 10:48:22 AM </Created>
//         <Key> b3998c81-af4d-45f2-b810-3f11e02c1d5b </Key>
//     </File>
//     <Summary>
//         SystemUtils.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using System;
using Puppy.Core.DateTimeUtils;

namespace Monkey.Core
{
    public static class SystemUtils
    {
        public static DateTime GetSystemTimeNow()
        {
            return DateTimeOffset.UtcNow.GetSystemTime();
        }

        public static DateTime GetSystemTime(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.UtcDateTime.GetDateTimeFromUtc(Constants.Setting.TimeZoneInfo);
        }

        public static DateTime GetSystemTime(this DateTime dateTimeUtc)
        {
            return dateTimeUtc.GetDateTimeFromUtc(Constants.Setting.TimeZoneInfo);
        }
    }
}