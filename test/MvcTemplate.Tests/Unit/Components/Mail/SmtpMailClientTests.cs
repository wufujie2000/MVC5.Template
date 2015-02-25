using MvcTemplate.Components.Mail;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mail
{
    public class SmtpMailClientTests
    {
        private SmtpMailClient client;

        public SmtpMailClientTests()
        {
            client = new SmtpMailClient();
        }

        #region Dispose()

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            client.Dispose();
            client.Dispose();
        }

        #endregion
    }
}
