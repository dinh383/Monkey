#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> Monkey.Auth.Domain.Exceptions </Project>
//     <File>
//         <Name> MonkeyException </Name>
//         <Created> 12/04/2017 09:19:27 AM </Created>
//         <Key> 81483575-3439-4eb8-b5ff-c7257bd731c6 </Key>
//     </File>
//     <Summary>
//         MonkeyException
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Monkey.Core.Exceptions
{
    public class MonkeyException : Exception
    {
        public MonkeyException(ErrorCode code, string message = "") : base(message)
        {
            Code = code;
        }

        public MonkeyException(ErrorCode code, Dictionary<string, object> additionalData) : this(code)
        {
            AdditionalData = additionalData;
        }

        public MonkeyException(ErrorCode code, string message, Dictionary<string, object> additionalData) : this(code, message)
        {
            AdditionalData = additionalData;
        }
        public ErrorCode Code { get; }

        [JsonExtensionData]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    }
}