using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;

namespace MvcTemplate.Components.Mail
{
    public class SmtpMailClient : IMailClient
    {
        private SmtpClient client;
        private Boolean disposed;
        private String sender;

        public SmtpMailClient()
        {
            sender = WebConfigurationManager.AppSettings["MailSenderAddress"];
            String password = WebConfigurationManager.AppSettings["MailSenderPassword"];

            String host = WebConfigurationManager.AppSettings["MailSmtpHost"];
            Int32 port = Int32.Parse(WebConfigurationManager.AppSettings["MailSmtpPort"]);

            client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(sender, password),
                EnableSsl = true
            };
        }

        public void Send(String to, String subject, String body)
        {
            client.Send(CreateEmail(to, subject, body));
        }

        private MailMessage CreateEmail(String to, String subject, String body)
        {
            MailMessage email = new MailMessage(sender, to, subject, body);
            email.SubjectEncoding = Encoding.UTF8;
            email.BodyEncoding = Encoding.UTF8;

            return email;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;

            client.Dispose();
            client = null;

            disposed = true;
        }
    }
}
