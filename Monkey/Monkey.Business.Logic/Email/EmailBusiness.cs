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

using Hangfire;
using Monkey.Core.Configs;
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
            string domainUrl = System.Web.HttpContext.Current.Request.GetDomain();

            string activeLink = $"<a href='{domainUrl}/portal/auth/active/{activeToken}'>Confirm Account</a>";

            string subject = "Confirm your account";

            string html = $"Please confirm your account via this link: {activeLink}";

            BackgroundJob.Enqueue(() => SendEmail(email, subject, html));
        }

        public void SendEmail(string email, string subject, string html)
        {
            var from = new EmailAddress(SystemConfig.SendGrid.SenderDisplayEmail, SystemConfig.SendGrid.SenderDisplayName);

            var to = new EmailAddress(email);

            var text = Regex.Replace(html, "<[^>]*>", string.Empty);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, text, html);
            
            var client = new SendGridClient(SystemConfig.SendGrid.Key);

            client.SendEmailAsync(msg).Wait();
        }
    }
}