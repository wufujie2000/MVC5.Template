using NUnit.Framework;
using Template.Components.Security;

namespace Template.Tests.Tests.Components.Security
{
    [TestFixture]
    public class BCrypterTests
    {
        #region Method: Hash(String value)

        [Test]
        public void Hash_Hashes()
        {
            Assert.IsTrue(BCrypter.Verify("Test", BCrypter.Hash("Test")));
        }

        #endregion

        #region Method: HashPassword(String value)

        [Test]
        public void HashPassword_Hashes()
        {
            Assert.IsTrue(BCrypter.Verify("Test", BCrypter.HashPassword("Test")));
        }

        #endregion
    }
}
