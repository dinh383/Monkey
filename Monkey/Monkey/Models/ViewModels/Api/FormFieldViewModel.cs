#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> FormFieldViewModel.cs </Name>
//         <Created> 24 Apr 17 1:04:48 AM </Created>
//         <Key> 3dbd6889-eaa0-4fac-807a-bfcfb6de6d72 </Key>
//     </File>
//     <Summary>
//         FormFieldViewModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Newtonsoft.Json;

namespace Monkey.Models.ViewModels.Api
{
    public class FormFieldViewModel
    {
        [JsonProperty(PropertyName = "minlength", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinLength { get; set; }

        [JsonProperty(PropertyName = "maxlength", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxLength { get; set; }

        public string Name { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Required { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }
}