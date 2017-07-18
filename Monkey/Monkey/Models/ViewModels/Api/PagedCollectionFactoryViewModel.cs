#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> PagedCollectionFactoryViewModel.cs </Name>
//         <Created> 24 Apr 17 1:13:09 AM </Created>
//         <Key> abf32d43-d90d-4ea3-ba9d-6f186c4ba149 </Key>
//     </File>
//     <Summary>
//         PagedCollectionFactoryViewModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace Monkey.Models.ViewModels.Api
{
    public class PagedCollectionFactoryViewModel<T>
    {
        private readonly string _hrefPattern;
        private readonly ILinkViewModel _meta;

        public PagedCollectionFactoryViewModel(ILinkViewModel meta, string hrefPattern)
        {
            _meta = meta;
            _hrefPattern = hrefPattern;
        }

        public PagedCollectionViewModel<T> CreateFrom(ICollection<T> items, int skip, int take, int total)
        {
            return new PagedCollectionViewModel<T>
            {
                Meta = _meta,
                HrefPattern = _hrefPattern,
                Take = take,
                Skip = skip,
                Total = total,
                Items = items,
                First = GetFirstLink(take),
                Last = GetLastLink(total, take),
                Next = GetNextLink(total, skip, take),
                Previous = GetPreviousLink(total, skip, take)
            };
        }

        private ILinkViewModel GetFirstLink(int take)
        {
            var newLink = new PlaceholderLinkViewModel(_meta);
            SafeSetValue(newLink.Values, nameof(take), take);
            SafeSetValue(newLink.Values, "skip", 0);
            newLink.Href = GetNewHrefByValues(newLink.Values);
            return newLink;
        }

        private ILinkViewModel GetLastLink(int total, int take)
        {
            if (total <= take)
                return null;

            var skip = Math.Ceiling((total - (double)take) / take) * take;

            var newLink = new PlaceholderLinkViewModel(_meta);
            SafeSetValue(newLink.Values, nameof(take), take);
            SafeSetValue(newLink.Values, nameof(skip), skip);

            newLink.Href = GetNewHrefByValues(newLink.Values);

            return newLink;
        }

        private ILinkViewModel GetNextLink(int total, int skip, int take)
        {
            var nextPage = skip + take;

            if (nextPage >= total)
                return null;

            var newLink = new PlaceholderLinkViewModel(_meta);
            SafeSetValue(newLink.Values, nameof(take), take);
            SafeSetValue(newLink.Values, nameof(skip), nextPage);
            newLink.Href = GetNewHrefByValues(newLink.Values);

            return newLink;
        }

        private ILinkViewModel GetPreviousLink(int total, int skip, int take)
        {
            if (skip == 0)
                return null;

            if (skip > total)
                return GetLastLink(total, take);

            var previousPage = Math.Max(skip - take, 0);

            if (previousPage <= 0)
                return GetFirstLink(take);

            var newLink = new PlaceholderLinkViewModel(_meta);
            SafeSetValue(newLink.Values, nameof(take), take);
            SafeSetValue(newLink.Values, nameof(skip), previousPage);
            newLink.Href = GetNewHrefByValues(newLink.Values);

            return newLink;
        }

        private void SafeSetValue(RouteValueDictionary routeValueDictionary, string key, object value)
        {
            if (routeValueDictionary.ContainsKey(key))
                routeValueDictionary.Remove(key);
            routeValueDictionary.Add(key, value);
        }

        private string GetNewHrefByValues(RouteValueDictionary routeValueDictionary)
        {
            var href = _hrefPattern;
            foreach (var newLinkValue in routeValueDictionary)
            {
                var hrefKey = "{" + newLinkValue.Key + "}";
                href = href.Replace(hrefKey, newLinkValue.Value.ToString());
            }

            return href;
        }
    }
}