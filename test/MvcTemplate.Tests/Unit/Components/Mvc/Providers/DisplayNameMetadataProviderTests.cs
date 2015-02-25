using MvcTemplate.Components.Mvc;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DisplayNameMetadataProviderTests
    {
        #region Method: GetMetadataForProperty(Func<Object> modelAccessor, Type containerType, String propertyName)

        [Fact]
        public void GetMetadataForProperty_SetsDisplayProperty()
        {
            DisplayNameMetadataProvider provider = new DisplayNameMetadataProvider();

            String actual = provider.GetMetadataForProperty(null, typeof(RoleView), "Name").DisplayName;
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Name");

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
