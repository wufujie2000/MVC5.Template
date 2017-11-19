using MvcTemplate.Components.Mvc;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DisplayNameMetadataProviderTests
    {
        #region GetMetadataForProperty(Func<Object> modelAccessor, Type containerType, String propertyName)

        [Fact]
        public void GetMetadataForProperty_SetsDisplayName()
        {
            String actual = new DisplayNameMetadataProvider().GetMetadataForProperty(null, typeof(RoleView), "Title").DisplayName;
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Title");

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetMetadataForType(Func<Object> modelAccessor, Type modelType)

        [Fact]
        public void GetMetadataForType_SetsDisplayNameToNull()
        {
            Assert.Null(new DisplayNameMetadataProvider().GetMetadataForType(null, typeof(String)).DisplayName);
        }

        #endregion
    }
}
