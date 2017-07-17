#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> DevelopersConfigModel.cs </Name>
//         <Created> 17/07/17 11:03:19 PM </Created>
//         <Key> f148fc57-541b-428a-a489-c909d8c2dca3 </Key>
//     </File>
//     <Summary>
//         DevelopersConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.ConfigModels
{
    public class DevelopersConfigModel
    {
        /// <summary>
        ///     Access Key read from URI 
        /// </summary>
        /// <remarks> Empty is allow <c> Anonymous </c> </remarks>
        public string AccessKey { get; set; }

        /// <summary>
        ///     Url of Api Document 
        /// </summary>
        /// <remarks> Start and End with <c> "/" </c>. Ex: /developers/doc/ </remarks>
        public string ApiDocumentUrl { get; set; }

        /// <summary>
        /// Api Document Name. Ex: api
        /// </summary>
        public string ApiDocumentName { get; set; }

        /// <summary>
        /// Api Document Title. Ex: Monkey API
        /// </summary>
        public string ApiDocumentTitle { get; set; }

        /// <summary>
        ///     Api Document Json File Name . Ex: api
        /// </summary>
        public string ApiDocumentJsonFile { get; set; }

        /// <summary>
        ///     Hangfire Dashboard Url. Ex: /developers/job 
        /// </summary>
        /// <remarks> Start with <c> "/" </c> but end with <c> empty </c> </remarks>
        public string HangfireDashboardUrl { get; set; }
    }
}