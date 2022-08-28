using Microsoft.Extensions.Configuration;
using ShnoFeeh.API.Core.Dto;
using System.Collections.Generic;
using System.IO;

namespace ShnoFeeh.API.Core.Common
{
    public class Email
    {
        IConfiguration _iconfiguration;
        public Email(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }
        public EmailAddressDto CreateEmailObject(string userName, string email, string subject, string url, string emailBody, string emailType = "")
        {
            string body = this.CreateEmailBody(emailType, userName, email, subject, url, emailBody);

            EmailAddressDto message = new EmailAddressDto();
            message.Subject = subject;

            // TO EMAIL ADDRESS INFO
            EmailAddress emailAddress = new EmailAddress();
            emailAddress.Name = userName;
            emailAddress.Address = email;
            List<EmailAddress> emailAddresses = new List<EmailAddress>();
            emailAddresses.Add(emailAddress);
            message.ToAddresses = emailAddresses;

            // FROM EMAIL ADDRESS INFO
            List<EmailAddress> emailAddresses1 = new List<EmailAddress>();
            EmailAddress emailAddress1 = new EmailAddress();
            emailAddress1.Name = "ShnoFeeh Support";
            emailAddress1.Address = _iconfiguration.GetSection("Smtp").GetSection("emailusername").Value;
            List<EmailAddress> emailAddresseslist = new List<EmailAddress>();
            emailAddresseslist.Add(emailAddress1);
            message.MessageBody = body;
            message.FromAddresses = emailAddresseslist;
            return message;

        }
        private string CreateEmailBody(string emailType, string userName, string email, string subject, string url, string emailBody)
        {
            string body = string.Empty;
            string EmailTemplate = string.Empty;

            switch (emailType)
            {
                case "forAction":

                    EmailTemplate = "./EmailTemplates/forAction.html";
                    break;

                case "forInfo":
                    EmailTemplate = "./EmailTemplates/forInfo.html";
                    break;

                default:
                    EmailTemplate = "./EmailTemplates/forAction.html";
                    break;
            }

            using (StreamReader reader = new StreamReader(EmailTemplate))
            {
                body = reader.ReadToEnd();
            }


            body = body.Replace("{UserName}", userName).Replace("{title}", subject).
                Replace("{URL}", url)
                .Replace("{Text1}", emailBody)
                 .Replace("{buttonText}", "click here").Replace("{logo}", "https://shnofeeh-storage.s3-ap-southeast-1.amazonaws.com/website/Logo.png");

            return body;

        }
    }
}
