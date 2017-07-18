#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> Resource.cs </Name>
//         <Created> 24 Apr 17 1:05:53 AM </Created>
//         <Key> b2f2acdd-380c-419b-992f-5252286c2571 </Key>
//     </File>
//     <Summary>
//         Resource.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Newtonsoft.Json;

namespace Monkey.Models.ViewModels.Api
{
    public abstract class ResourceViewModel
    {
        [JsonProperty(Order = -2)]
        public ILinkViewModel Meta { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FormViewModel[] Forms { get; set; }
    }
}