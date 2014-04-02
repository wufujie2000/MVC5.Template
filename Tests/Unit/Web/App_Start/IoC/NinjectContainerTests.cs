using NUnit.Framework;
using System;
using System.Web.Mvc;
using Template.Data.Core;
using Template.Web.IoC;

namespace Template.Tests.Unit.Web.App_Start.IoC
{
    [TestFixture]
    public class NinjectContainerTests
    {
        #region Static method: RegisterModules(params NinjectModule[] modules)

        [Test]
        public void RegisterModules_SetsDependencyResolver()
        {
            NinjectContainer.RegisterModules(new MainModule());

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
            var actualInstace = NinjectContainer.Resolve<AContext>();

            Assert.IsInstanceOf(expectedType, actualInstace);
        }

        [Test]
        public void Resolve_OnNotBindedReturnsNull()
        {
            Assert.IsNull(NinjectContainer.Resolve<IDisposable>());
        }

        #endregion
    }
}
