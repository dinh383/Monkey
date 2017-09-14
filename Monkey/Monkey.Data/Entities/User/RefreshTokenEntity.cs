#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> RefreshTokenEntity.cs </Name>
//         <Created> 13/09/17 9:56:51 PM </Created>
//         <Key> df40885c-9a75-4219-ae6e-bac743d01162 </Key>
//     </File>
//     <Summary>
//         RefreshTokenEntity.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Puppy.Web.HttpRequestDetection.Device;
using System;

namespace Monkey.Data.Entities.User
{
    public class RefreshTokenEntity : Entity
    {
        public string RefreshToken { get; set; }

        public int TotalUsage { get; set; } = 1;

        public DateTimeOffset? ExpireOn { get; set; }

        public int UserId { get; set; }

        public virtual UserEntity User { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceType DeviceType { get; set; }

        // Marker

        public string MarkerName { get; set; }

        public string MarkerVersion { get; set; }

        // OS

        public string OsName { get; set; }

        public string OsVersion { get; set; }

        // Engine

        public string EngineName { get; set; }

        public string EngineVersion { get; set; }

        // Browser

        public string BrowserName { get; set; }

        public string BrowserVersion { get; set; }

        // Location

        public string IpAddress { get; set; }

        public string CityName { get; set; }

        public int? CityGeoNameId { get; set; }

        public string CountryName { get; set; }

        public int? CountryGeoNameId { get; set; }

        public string CountryIsoCode { get; set; }

        public string ContinentName { get; set; }

        public int? ContinentGeoNameId { get; set; }

        public string ContinentCode { get; set; }

        public string TimeZone { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public int? AccuracyRadius { get; set; }

        public string PostalCode { get; set; }

        // Others

        public string UserAgent { get; set; }

        public string DeviceHash { get; set; }


        // Client

        public int ClientId { get; set; }
    }
}