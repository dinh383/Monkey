#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Logic </Project>
//     <File>
//         <Name> EmailBusiness.cs </Name>
//         <Created> 09/10/17 8:45:07 PM </Created>
//         <Key> ceec385c-1e44-4372-9035-0c196a2e2dba </Key>
//     </File>
//     <Summary>
//         EmailBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Puppy.DependencyInjection.Attributes;
using Puppy.Web.HttpUtils;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Text.RegularExpressions;

namespace Monkey.Business.Logic.Email
{
    [PerRequestDependency(ServiceType = typeof(IEmailBusiness))]
    public class EmailBusiness : IEmailBusiness
    {
        public void SendActiveAccountByEmail(string activeToken, string email, string roleName, TimeSpan expireIn)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("demo.sendmail913@gmail.com", "Owl");
            var to = new EmailAddress(email);
            string domainUrl = System.Web.HttpContext.Current.Request.GetDomain();
            string activeLink = $"<a href=\"{domainUrl}/portal/active/{activeToken}\">Confirm Account</a>";
            string htmlContent = $"Please confirm your account via this link: {activeLink}";
            var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, "Confirm your account", plainTextContent, htmlContent);
            client.SendEmailAsync(msg).Wait();
        }
    }
}