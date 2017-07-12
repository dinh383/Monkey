#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> PagedCollectionParameters.cs </Name>
//         <Created> 24 Apr 17 1:15:01 AM </Created>
//         <Key> 7bbbc234-4dcf-4df6-8690-ced4a8906583 </Key>
//     </File>
//     <Summary>
//         PagedCollectionParameters.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

namespace Monkey.Core.ViewModels.Api
{
    public class PagedCollectionParameters
    {
        public int? Offset { get; set; }

        public int? Limit { get; set; }
    }
}