using NUnit.Framework;
using System;
using Template.Components.Mvc;
using Template.Objects;
using Template.Resources;

namespace Template.Tests.Components.Mvc
{
    [TestFixture]
    public class DisplayNameMetadataProviderTests
    {
        #region Method: GetMetadataForProperty(Func<Object> modelAccessor, Type containerType, String propertyName)

        [Test]
        public void GetMetadataForProperty_SetsDisplayProperty()
        {
            DisplayNameMetadataProvider provider = new DisplayNameMetadataProvider();
            Type containerType = typeof(RoleView);
            String propertyName = "Name";

            String expected = ResourceProvider.GetPropertyTitle(containerType, propertyName);
            String actual = provider.GetMetadataForProperty(null, containerType, propertyName).DisplayName;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
