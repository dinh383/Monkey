#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ElasticConfigModel.cs </Name>
//         <Created> 18/07/17 12:03:17 PM </Created>
//         <Key> 9ea0e466-03ef-4eb0-896d-9c4b5b7d0c1f </Key>
//     </File>
//     <Summary>
//         ElasticConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.ConfigModels
{
    public class ElasticConfigModel
    {
        /// <summary>
        ///     Elastic Search Endpoint 
        /// </summary>
        public string ConnectionString { get; set; }
    }
}