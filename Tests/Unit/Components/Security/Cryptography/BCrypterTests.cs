using MvcTemplate.Components.Security;
using NUnit.Framework;
using System;

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
            String value = "Test";
            String hash = crypter.Hash(value);

            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(value, hash));
        }

        #endregion

        #region Method: HashPassword(String value)

        [Test]
        public void HashPassword_Hashes()
        {
            String value = "Test";
            String hash = crypter.HashPassword(value);

            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(value, hash));
        }

        #endregion

        #region Method: Verify(String value, String hash)

        [Test]
        public void Verify_VerifiesHash()
        {
            String value = "Test";
            String hash = BCrypt.Net.BCrypt.HashString(value, 4);

            Assert.IsTrue(crypter.Verify(value, hash));
        }

        #endregion
    }
}
