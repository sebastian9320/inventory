namespace Shop.Web.Helpers
{
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Configuration;
    using MimeKit;
using System.Net.Mail;
using System.Net;

    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration configuration;

        public MailHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendMail(string to, string subject, string body)
        {
            var from = this.configuration["Mail:From"];
            var smtp = this.configuration["Mail:Smtp"];
            var port = this.configuration["Mail:Port"];
            var password = this.configuration["Mail:Password"];

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            var client = new System.Net.Mail.SmtpClient
            {
                Host = smtp,
                Port = int.Parse(port),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, password)

            };
            using (var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                client.Send(message);
            }
        }
    }
    }

