#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> ILinkViewModel.cs </Name>
//         <Created> 24 Apr 17 12:56:59 AM </Created>
//         <Key> 19ef9707-265d-4ab2-b985-edd907535350 </Key>
//     </File>
//     <Summary>
//         ILinkViewModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Newtonsoft.Json;

namespace Monkey.Core.ViewModels.Api
{
    public interface ILinkViewModel
    {
        string Href { get; set; }

        [JsonProperty(PropertyName = "rel", NullValueHandling = NullValueHandling.Ignore)]
        string[] Relations { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Method { get; set; }
    }
}