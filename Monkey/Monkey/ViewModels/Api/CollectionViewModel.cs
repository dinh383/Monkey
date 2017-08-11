#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <DisplayUrl> http://topnguyen.net/ </DisplayUrl>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> CollectionViewModel.cs </Name>
//         <Created> 24 Apr 17 1:05:24 AM </Created>
//         <Key> 13589257-5e1a-4121-8340-c73a422b2e38 </Key>
//     </File>
//     <Summary>
//         CollectionViewModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using System.Collections.Generic;

namespace Monkey.ViewModels.Api
{
    public class CollectionViewModel<T> : ResourceViewModel
    {
        public ICollection<T> Items { get; set; }
    }
}