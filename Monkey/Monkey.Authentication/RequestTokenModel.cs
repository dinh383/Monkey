﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> RequestTokenModel.cs </Name>
//         <Created> 04/09/17 10:34:44 PM </Created>
//         <Key> 1754adc4-14bb-4d8f-8b79-5fa70f617f07 </Key>
//     </File>
//     <Summary>
//         RequestTokenModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Monkey.Authentication
{
    public class RequestTokenModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public GrantType GrantType { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string RefreshToken { get; set; }
    }
}