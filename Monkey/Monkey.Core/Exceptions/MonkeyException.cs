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

namespace Monkey.Core.Exceptions
{
    public class MonkeyException : Exception
    {
        public MonkeyException(ErrorCode code, string message = "") : base(message)
        {
            Code = code;
        }

        public MonkeyException(ErrorCode code, params (string Key, object Value)[] arrayKeyValue)
        {
            Code = code;
            ArrayKeyValue = arrayKeyValue;
        }

        public MonkeyException(ErrorCode code, string message,
            params (string Key, object Value)[] arrayKeyValue) : base(message)
        {
            Code = code;
            ArrayKeyValue = arrayKeyValue;
        }

        public MonkeyException(ErrorCode code, string message, Exception innerException,
            params (string Key, object Value)[] arrayKeyValue) : base(message, innerException)
        {
            Code = code;
            ArrayKeyValue = arrayKeyValue;
        }

        public ErrorCode Code { get; }

        public (string Key, object Value)[] ArrayKeyValue { get; }
    }
}