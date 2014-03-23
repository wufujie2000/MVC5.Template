using NUnit.Framework;
using Template.Objects;

namespace Template.Tests.Tests.Objects.Models
{
    [TestFixture]
    public class LogTests
    {
        #region Constructor: Log()

        [Test]
        public void Log_LeavesNullMessage()
        {
            Assert.IsNull(new Log().Message);
        }

        #endregion

        #region Constructor: Log(String message)

        [Test]
        public void Log_SetsMessage()
        {
            Assert.AreEqual("Test", new Log("Test").Message);
        }

        #endregion
    }
}
