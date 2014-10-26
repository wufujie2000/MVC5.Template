using System;

namespace MvcTemplate.Components.Mail
{
    public interface IMailClient : IDisposable
    {
        void Send(String email, String subject, String body);
    }
}
