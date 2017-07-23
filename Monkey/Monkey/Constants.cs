#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> Monkey.Auth.Domain </Project>
//     <File>
//         <Name> Constants </Name>
//         <Created> 08/04/2017 11:31:32 PM </Created>
//         <Key> dc620a3e-88e2-4916-b998-b44e6c48db03 </Key>
//     </File>
//     <Summary>
//         Constants
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Monkey
{
    public static class Constants
    {
        public static class ViewDataKey
        {
            // Common
            public const string Title = nameof(Title);

            public const string Description = nameof(Description);

            public const string PageUrl = nameof(PageUrl);

            public const string ImageUrl = nameof(ImageUrl);

            // Facebook
            public const string FacebookType = nameof(FacebookType);

            // Twitter
            public const string TwitterType = nameof(TwitterType);

            public const string TwitterSite = nameof(TwitterSite);
        }
    }
}