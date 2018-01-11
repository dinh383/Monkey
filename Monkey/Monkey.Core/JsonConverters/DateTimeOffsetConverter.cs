#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2018 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> DateTimeOffsetConverter.cs </Name>
//         <Created> 11/01/2018 10:07:06 AM </Created>
//         <Key> 2f646729-fc0c-4d9c-8cef-b819ea74b36c </Key>
//     </File>
//     <Summary>
//         DateTimeOffsetConverter.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Configs;
using Newtonsoft.Json;
using Puppy.Core.TypeUtils;
using System;

namespace Monkey.Core.JsonConverters
{
    public class DateTimeOffsetConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var dateTimeOffset = (DateTimeOffset)value;
                writer.WriteValue(dateTimeOffset.ToString(SystemConfig.SystemDateTimeFormat));
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTimeOffset? dateTimeOffset = reader.Value.ToString().ToSystemDateTime();

            return dateTimeOffset;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.GetNotNullableType() == typeof(DateTimeOffset);
        }
    }
}