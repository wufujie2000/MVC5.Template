using Moq;
using NUnit.Framework;
using Template.Components.Mvc.SiteMap;

namespace Template.Tests.Unit.Components.Mvc.SiteMap
{
    [TestFixture]
    public class MvcSiteMapFactoryTests
    {
        [TearDown]
        public void TearDown()
        {
            MvcSiteMapFactory.Provider = null;
        }

        #region Static property: Provider

        [Test]
        public void Provider_GetsAndSetsProviderInstance()
        {
            IMvcSiteMapProvider expected = new Mock<IMvcSiteMapProvider>().Object;
            MvcSiteMapFactory.Provider = expected;

            IMvcSiteMapProvider actual = MvcSiteMapFactory.Provider;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
