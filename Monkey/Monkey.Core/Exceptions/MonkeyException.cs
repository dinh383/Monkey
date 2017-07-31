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

        public MonkeyException(ErrorCode code, params KeyValuePair<string, object>[] arrayKeyValue)
        {
            Code = code;
            ArrayKeyValue = arrayKeyValue;
        }

        public MonkeyException(ErrorCode code, string message,
            params KeyValuePair<string, object>[] arrayKeyValue) : base(message)
        {
            Code = code;
            ArrayKeyValue = arrayKeyValue;
        }

        public MonkeyException(ErrorCode code, string message, Exception innerException,
            params KeyValuePair<string, object>[] arrayKeyValue) : base(message, innerException)
        {
            Code = code;
            ArrayKeyValue = arrayKeyValue;
        }

        public ErrorCode Code { get; }

        public KeyValuePair<string, object>[] ArrayKeyValue { get; }
    }
}