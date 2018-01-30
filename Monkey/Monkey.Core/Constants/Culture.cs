using Puppy.Core.StringUtils;

namespace Monkey.Core.Constants
{
    public static class Culture
    {
        public static readonly string CookieName = $"{nameof(Monkey)}Culture".GetSha256();

        public const string Vietnamese = "vi-VN";

        public const string English = "en-US";
    }
}