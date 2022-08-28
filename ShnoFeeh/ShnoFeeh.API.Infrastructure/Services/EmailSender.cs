using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using ShnoFeeh.API.Core.Common;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        //public EmailSender()
        //{
        //    _configuration = new ConfigurationBuilder()
        //          .AddJsonFile("appsettings.json")
        //          .Build();
        //}
        public EmailSender(IAppLogger appLogger)
        {
            _configuration = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json")
                  .Build();
            _appLogger = appLogger;
        }
        public async Task<bool> SendEmailAsync(EmailAddressDto emailDto)
        {
            try
            {
                var smtp = _configuration.GetValue<string>("Smtp:Server");
                string port = _configuration.GetValue<string>("Smtp:Port"); ;
                var username = _configuration.GetValue<string>("Smtp:emailusername");
                var password = _configuration.GetValue<string>("Smtp:emailpassword");
                var message = new MimeMessage();
                message.To.AddRange(emailDto.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                message.From.AddRange(emailDto.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.Subject = emailDto.Subject;
                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = emailDto.MessageBody
                };

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {

                    //The last parameter here is to use SSL (Which you should!)emi  
                    emailClient.Connect(smtp, Convert.ToInt32(port), MailKit.Security.SecureSocketOptions.Auto);
                    
                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    
                    emailClient.Authenticate(username, password);

                    await emailClient.SendAsync(message);

                    emailClient.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                //Get the first stack frame
                StackFrame frame = st.GetFrame(0);
                ExceptionLogDto exLog = new ExceptionLogDto
                {
                    ExceptionLogId = new RandomSG().GetString(),
                    ExceptionType = ex.GetType().FullName.ToString(),
                    ExceptionInnerException = ex.InnerException == null ? "" : ex.InnerException.ToString(),
                    ExceptionMessage = ex.Message,
                    ExceptionSeverity = "ERROR",
                    ExceptionFileName = frame.GetFileName(), //Get the file name
                    ExceptionLineNumber = frame.GetFileLineNumber(),  //Get the line number
                    ExceptionColumnNumber = frame.GetFileColumnNumber(), //Get the column number                      
                    ExceptionMethodName = ex.TargetSite.ReflectedType.FullName // Get the method name

                };
                await _appLogger.LogInformationAsync(exLog);
                return false;
            }

        }
    }
}
