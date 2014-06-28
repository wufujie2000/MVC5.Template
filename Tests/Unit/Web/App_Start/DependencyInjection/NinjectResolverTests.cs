using MvcTemplate.Data.Core;
using MvcTemplate.Web.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Tests.Unit.Web.DependencyInjection
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
            Type expectedType = typeof(AContext);
            Object actualInstance = resolver.GetService(expectedType);

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
            Type expectedType = typeof(AContext);
            IEnumerable<Object> services = resolver.GetServices(expectedType);

            foreach (Object actualInstace in services)
                Assert.IsInstanceOf(expectedType, actualInstace);
        }

        [Test]
        public void GetServices_OnNotBindedReturnsNull()
        {
            CollectionAssert.IsEmpty(resolver.GetServices(typeof(IDisposable)));
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            resolver.Dispose();
            resolver.Dispose();
        }

        #endregion
    }
}
