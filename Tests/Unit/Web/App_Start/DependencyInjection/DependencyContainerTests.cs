using NUnit.Framework;
using System;
using System.Web.Mvc;
using Template.Data.Core;
using Template.Web.DependencyInjection;
using Template.Web.DependencyInjection.Ninject;

namespace Template.Tests.Unit.Web.DependencyInjection
{
    [TestFixture]
    public class DependencyContainerTests
    {
        #region Static method: RegisterModules(params NinjectModule[] modules)

        [Test]
        public void RegisterModules_SetsDependencyResolver()
        {
            DependencyContainer.RegisterResolver(new NinjectResolver(new MainModule()));

            var expectedType = typeof(NinjectResolver);
            var actualInstace = DependencyResolver.Current;

            Assert.IsInstanceOf(expectedType, actualInstace);
        }

        #endregion

        #region Static method: Resolve<T>()

        [Test]
        public void Resolve_ResolvesType()
        {
            var expectedType = typeof(AContext);
            var actualInstace = DependencyContainer.Resolve<AContext>();

            Assert.IsInstanceOf(expectedType, actualInstace);
        }

        [Test]
        public void Resolve_OnNotBindedReturnsNull()
        {
            Assert.IsNull(DependencyContainer.Resolve<IDisposable>());
        }

        #endregion
    }
}
