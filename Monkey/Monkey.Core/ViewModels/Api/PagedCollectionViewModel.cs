#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> PagedCollectionViewModel.cs </Name>
//         <Created> 24 Apr 17 1:12:21 AM </Created>
//         <Key> aed6eed0-bbc9-42a2-b6c7-5b6bd99e6a2c </Key>
//     </File>
//     <Summary>
//         PagedCollectionViewModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

namespace Monkey.Core.ViewModels.Api
{
    public class PagedCollectionViewModel<T> : CollectionViewModel<T>
    {
        public string HrefPattern { get; set; }

        public int Total { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public ILinkViewModel First { get; set; }

        public ILinkViewModel Previous { get; set; }

        public ILinkViewModel Next { get; set; }

        public ILinkViewModel Last { get; set; }
    }
}