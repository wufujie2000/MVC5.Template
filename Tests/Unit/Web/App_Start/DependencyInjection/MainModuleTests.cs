using Ninject;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using Template.Components.Logging;
using Template.Components.Mvc;
using Template.Components.Security;
using Template.Data.Core;
using Template.Services;
using Template.Web.DependencyInjection;

namespace Template.Tests.Unit.Web.DependencyInjection
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
        public void Load_BindsILogger()
        {
            AssertBind<ILogger, Logger>();
        }

        [Test]
        public void Load_BindsAContext()
        {
            AssertBind<AContext, Context>();
        }

        [Test]
        public void Load_BindsIUnitOfWork()
        {
            AssertBind<IUnitOfWork, UnitOfWork>();
        }

        [Test]
        public void Load_BindsIEntityLogger()
        {
            AssertBind<IEntityLogger, EntityLogger>();
        }

        [Test]
        public void Load_BindsIExceptionFilter()
        {
            AssertBind<IExceptionFilter, ExceptionFilter>();
        }

        [Test]
        public void Load_BindsIRoleProvider()
        {
            AssertBind<IRoleProvider, RoleProvider>();
        }

        [Test]
        public void Load_BindsIRoleProviderToConstant()
        {
            IRoleProvider expected = kernel.Get<IRoleProvider>();
            IRoleProvider actual = kernel.Get<IRoleProvider>();

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Load_BindsIMvcSiteMapParser()
        {
            AssertBind<IMvcSiteMapParser, MvcSiteMapParser>();
        }

        [Test]
        [Ignore]
        public void Load_BindsIMvcSiteMapProvider()
        {
            AssertBind<IMvcSiteMapProvider, MvcSiteMapProvider>();
        }

        [Test]
        public void Load_BindsIAuthService()
        {
            AssertBind<IAuthService, AuthService>();
        }

        [Test]
        public void Load_BindsIHomeService()
        {
            AssertBind<IHomeService, HomeService>();
        }

        [Test]
        public void Load_BindsIRolesService()
        {
            AssertBind<IRolesService, RolesService>();
        }

        [Test]
        public void Load_BindsIProfileService()
        {
            AssertBind<IProfileService, ProfileService>();
        }

        [Test]
        public void Load_BindsIAccountsService()
        {
            AssertBind<IAccountsService, AccountsService>();
        }

        #endregion

        #region Test helpers

        private void AssertBind<TAbstraction, TImplementation>()
        {
            Type actual = kernel.Get<TAbstraction>().GetType();
            Type expected = typeof(TImplementation);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
