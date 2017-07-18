#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> LinkViewModel.cs </Name>
//         <Created> 24 Apr 17 12:57:27 AM </Created>
//         <Key> fddb64c4-2447-4b5d-afe7-fe7abc6ccdf2 </Key>
//     </File>
//     <Summary>
//         LinkViewModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

namespace Monkey.Models.ViewModels.Api
{
    public class LinkViewModel : ILinkViewModel
    {
        public string Href { get; set; }

        public string[] Relations { get; set; }

        public string Method { get; set; }
    }
}