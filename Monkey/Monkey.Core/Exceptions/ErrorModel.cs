#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> Monkey.Core.Models </Project>
//     <File>
//         <Name> ErrorModel </Name>
//         <Created> 12/04/2017 09:21:24 AM </Created>
//         <Key> 9626e038-c848-4955-8ce2-4f20e371d277 </Key>
//     </File>
//     <Summary>
//         ErrorModel
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using EnumsNET;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Core.Exceptions
{
    public class ErrorModel
    {
        public ErrorModel()
        {
        }

        public ErrorModel(ErrorCode code, string message)
        {
            Code = code;
            Message = string.IsNullOrWhiteSpace(message) ? code.AsString(EnumFormat.Description) : message;
            Module = Enums.GetMember<ErrorCode>(code.ToString()).Value.GetAttributes().Get<DisplayAttribute>().GetGroupName();
        }

        public ErrorModel(ErrorCode code, string message, Dictionary<string, object> additionalData) : this(code, message)
        {
            AdditionalData = additionalData;
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