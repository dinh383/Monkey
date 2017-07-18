#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> PlaceholderLinkViewModel.cs </Name>
//         <Created> 24 Apr 17 1:02:56 AM </Created>
//         <Key> 4bdc446c-caaa-43b8-9db8-9a8985d2a4cd </Key>
//     </File>
//     <Summary>
//         PlaceholderLinkViewModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Microsoft.AspNetCore.Routing;

namespace Monkey.ViewModels.Api
{
    public class PlaceholderLinkViewModel : ILinkViewModel
    {
        public PlaceholderLinkViewModel()
        {
        }

        public PlaceholderLinkViewModel(ILinkViewModel existing)
        {
            Href = existing.Href;
            Relations = existing.Relations;
            Method = existing.Method;

            var asPlaceholder = existing as PlaceholderLinkViewModel;
            if (asPlaceholder != null)
                Values = new RouteValueDictionary(asPlaceholder.Values);
        }

        public RouteValueDictionary Values { get; set; } = new RouteValueDictionary();

        public string Href { get; set; }
        public string[] Relations { get; set; }

        public string Method { get; set; }

        public static PlaceholderLinkViewModel ToResource(string endpoint, string id, string method = "GET",
            object values = null)
        {
            return new PlaceholderLinkViewModel
            {
                Href = endpoint,
                Method = method,
                Relations = new string[0],
                Values = new RouteValueDictionary(values)
                {
                    {nameof(id), id}
                }
            };
        }

        public static PlaceholderLinkViewModel ToCollection(string hrefPattern, string method = "GET",
            object values = null)
        {
            var placeholderLinkViewModel = new PlaceholderLinkViewModel
            {
                Method = method,
                Relations = new[] { "collection" },
                Values = new RouteValueDictionary(values)
            };

            placeholderLinkViewModel.Href = hrefPattern;
            foreach (var newLinkValue in placeholderLinkViewModel.Values)
            {
                var hrefKey = "{" + newLinkValue.Key + "}";
                placeholderLinkViewModel.Href =
                    placeholderLinkViewModel.Href.Replace(hrefKey, newLinkValue.Value.ToString());
            }

            return placeholderLinkViewModel;
        }
    }
}