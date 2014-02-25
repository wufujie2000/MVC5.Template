using NUnit.Framework;
using System;
using Template.Components.Mvc.Providers;
using Template.Objects;

namespace Template.Tests.Components.Mvc.Providers
{
    [TestFixture]
    public class DisplayNameMetadataProviderTests
    {
        private DisplayNameMetadataProvider provider;

        [SetUp]
        public void SetUp()
        {
            provider = new DisplayNameMetadataProvider();
        }

        #region Method: GetMetadataForProperty(Func<Object> modelAccessor, Type containerType, String propertyName)

        [Test]
        public void GetMetadataForProperty_SetsDisplayProperty()
        {
            Assert.AreEqual(
                Template.Resources.Views.ProfileView.Titles.Username,
                provider.GetMetadataForProperty(() => null, typeof(ProfileView), "Username").DisplayName);
        }

        [Test]
        public void GetMetadataForProperty_OnResourceNotFoundSetsToEmpty()
        {
            Assert.AreEqual(String.Empty, provider.GetMetadataForProperty(() => null, typeof(ProfileView), "Id").DisplayName);
        }

        #endregion
    }
}
