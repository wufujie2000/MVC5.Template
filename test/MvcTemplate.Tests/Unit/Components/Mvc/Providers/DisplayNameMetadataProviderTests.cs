using MvcTemplate.Objects;
using MvcTemplate.Resources;
using System;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DisplayNameMetadataProviderTests
    {
        #region Method: CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<Object> modelAccessor, Type modelType, String propertyName)

        [Fact]
        public void CreateMetadata_SetsDisplayName()
        {
            DisplayNameMetadataProviderProxy provider = new DisplayNameMetadataProviderProxy();

            String actual = provider.BaseCreateMetadata(Enumerable.Empty<Attribute>(), typeof(RoleView), null, typeof(String), "Title").DisplayName;
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Title");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateMetadata_OnNullContainerTypeDoesNotSetDisplayName()
        {
            DisplayNameMetadataProviderProxy provider = new DisplayNameMetadataProviderProxy();

            String actual = provider.BaseCreateMetadata(Enumerable.Empty<Attribute>(), null, null, typeof(String), "Name").DisplayName;

            Assert.Null(actual);
        }

        #endregion
    }
}
