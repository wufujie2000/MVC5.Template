using Ninject;
using NUnit.Framework;
using System;
using Template.Components.Mvc.SiteMap;
using Template.Components.Security;
using Template.Data.Core;
using Template.Data.Logging;
using Template.Services;
using Template.Web.DependencyInjection.Ninject;

namespace Template.Tests.Unit.Web.DependencyInjection.Ninject
{
    [TestFixture]
    public class MainModuleTests
    {
        private KernelBase kernel;
        private MainModule module;

        [SetUp]
        public void SetUp()
        {
            module = new MainModule();
            kernel = new StandardKernel(module);
        }

        #region Method: Load()

        [Test]
        public void Load_BindsAContext()
        {
            Type expected = typeof(Context);
            Type actual = kernel.Get<AContext>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIUnitOfWork()
        {
            Type expected = typeof(UnitOfWork);
            Type actual = kernel.Get<IUnitOfWork>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIEntityLogger()
        {
            Type expected = typeof(EntityLogger);
            Type actual = kernel.Get<IEntityLogger>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIRoleProvider()
        {
            Type expected = typeof(RoleProvider);
            Type actual = kernel.Get<IRoleProvider>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Ignore]
        public void Load_BindsIMvcSiteMapProvider()
        {
            Type expected = typeof(MvcSiteMapProvider);
            Type actual = kernel.Get<IMvcSiteMapProvider>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIAuthService()
        {
            Type expected = typeof(AuthService);
            Type actual = kernel.Get<IAuthService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIHomeService()
        {
            Type expected = typeof(HomeService);
            Type actual = kernel.Get<IHomeService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIRolesService()
        {
            Type expected = typeof(RolesService);
            Type actual = kernel.Get<IRolesService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIProfileService()
        {
            Type expected = typeof(ProfileService);
            Type actual = kernel.Get<IProfileService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIAccountsService()
        {
            Type expected = typeof(AccountsService);
            Type actual = kernel.Get<IAccountsService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
