using NUnit.Framework;
using System;
using Template.Data.Core;
using Template.Web.DependencyInjection.Ninject;

namespace Template.Tests.Unit.Web.DependencyInjection.Ninject
{
    [TestFixture]
    public class NinjectResolverTests
    {
        private NinjectResolver resolver;

        [SetUp]
        public void SetUp()
        {
            resolver = new NinjectResolver(new MainModule());
        }

        #region Method: GetService(Type serviceType)

        [Test]
        public void GetService_GetsService()
        {
            var expectedType = typeof(AContext);
            var actualInstance = resolver.GetService(expectedType);
            
            Assert.IsInstanceOf(expectedType, actualInstance);
        }

        [Test]
        public void GetService_OnNotBindedReturnsNull()
        {
            Assert.IsNull(resolver.GetService(typeof(IDisposable)));
        }

        #endregion

        #region Method: GetServices(Type serviceType)

        [Test]
        public void GetServices_GetsAllServices()
        {
            var expectedType = typeof(AContext);
            var services = resolver.GetServices(expectedType);

            foreach (var actualInstace in services)
                Assert.IsInstanceOf(expectedType, actualInstace);
        }

        [Test]
        public void GetServices_OnNotBindedReturnsNull()
        {
            CollectionAssert.IsEmpty(resolver.GetServices(typeof(IDisposable)));
        }

        #endregion
    }
}
