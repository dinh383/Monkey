#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> Monkey.Auth.Domain.ViewModels </Project>
//     <File>
//         <Name> ApiErrorViewModel </Name>
//         <Created> 12/04/2017 09:21:24 AM </Created>
//         <Key> 9626e038-c848-4955-8ce2-4f20e371d277 </Key>
//     </File>
//     <Summary>
//         ApiErrorViewModel
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using EnumsNET;
using Monkey.Core.Exceptions;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monkey.ViewModels.Api
{
    public class ApiErrorViewModel
    {
        public ApiErrorViewModel()
        {
        }

        public ApiErrorViewModel(ErrorCode code, string message)
        {
            Code = code;
            Message = string.IsNullOrWhiteSpace(message) ? code.AsString(EnumFormat.Description) : message;
            Module = code.AsString(EnumFormat.DisplayName);
        }

        public string Id { get; set; }

        /// <summary>
        ///     Unique Code for each Business 
        /// </summary>
        public ErrorCode Code { get; set; } = ErrorCode.Unknown;

        /// <summary>
        ///     Message description for Client App Developer 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Module of business 
        /// </summary>
        public string Module { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    }
}