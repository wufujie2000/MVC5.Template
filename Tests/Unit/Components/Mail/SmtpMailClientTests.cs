using MvcTemplate.Components.Mail;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Components.Mail
{
    [TestFixture]
    public class SmtpMailClientTests
    {
        private SmtpMailClient client;

        [SetUp]
        public void SetUp()
        {
            client = new SmtpMailClient();
        }

        #region Dispose()

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            client.Dispose();
            client.Dispose();
        }

        #endregion
    }
}
