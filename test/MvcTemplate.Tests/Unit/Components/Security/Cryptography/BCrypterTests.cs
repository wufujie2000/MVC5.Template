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
        public void Verify_OnNullHashAlwaysFails()
        {
            Assert.IsFalse(crypter.Verify("", null));
        }

        [Test]
        public void Verify_VerifiesHash()
        {
            Assert.IsTrue(crypter.Verify("Test", "$2a$04$tXfDH9cZGOqFbCV8CF1ik.kW8R7.UKpEi5G7P4K842As1DI1bwDxm"));
        }

        #endregion

        #region Method: VerifyPassword(String value, String passhash)

        [Test]
        public void VerifyPassword_OnNullHashAlwaysFails()
        {
            Assert.IsFalse(crypter.VerifyPassword("", null));
        }

        [Test]
        public void VerifyPassword_VerifiesPasshash()
        {
            Assert.IsTrue(crypter.VerifyPassword("Test", "$2a$13$g7QgmyFicKkyI4kiHM8XQ.LfBdpdcLUbw1tkr9.owCN5qY9eCj8Lq"));
        }

        #endregion
    }
}
