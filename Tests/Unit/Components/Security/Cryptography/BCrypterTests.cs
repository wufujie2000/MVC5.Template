using MvcTemplate.Components.Security;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [TestFixture]
    public class BCrypterTests
    {
        private BCrypter crypter;

        [SetUp]
        public void SetUp()
        {
            crypter = new BCrypter();
        }

        #region Method: Hash(String value)

        [Test]
        public void Hash_Hashes()
        {
            Assert.IsTrue(crypter.Verify("Test", crypter.Hash("Test")));
        }

        #endregion

        #region Method: HashPassword(String value)

        [Test]
        public void HashPassword_Hashes()
        {
            Assert.IsTrue(crypter.Verify("Test", crypter.HashPassword("Test")));
        }

        #endregion
    }
}
