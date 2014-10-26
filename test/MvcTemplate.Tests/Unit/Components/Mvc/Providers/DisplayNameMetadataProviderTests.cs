using MvcTemplate.Components.Mvc;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using NUnit.Framework;
using System;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class DisplayNameMetadataProviderTests
    {
        #region Method: GetMetadataForProperty(Func<Object> modelAccessor, Type containerType, String propertyName)

        [Test]
        public void GetMetadataForProperty_SetsDisplayProperty()
        {
            DisplayNameMetadataProvider provider = new DisplayNameMetadataProvider();

            String actual = provider.GetMetadataForProperty(null, typeof(RoleView), "Name").DisplayName;
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Name");

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
