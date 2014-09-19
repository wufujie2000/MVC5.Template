using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using MvcTemplate.Web.DependencyInjection;
using Ninject;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Web.DependencyInjection
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
        public void Load_BindsIAuditLogger()
        {
            AssertBind<IAuditLogger, AuditLogger>();
        }

        [Test]
        public void Load_BindsIMailClientToSmtpMailClient()
        {
            AssertBind<IMailClient, SmtpMailClient>();
        }

        [Test]
        public void Load_BindsIExceptionFilter()
        {
            AssertBind<IExceptionFilter, ExceptionFilter>();
        }

        [Test]
        public void Load_BindsIAuthorizationProvider()
        {
            AssertBind<IAuthorizationProvider, AuthorizationProvider>();
        }

        [Test]
        public void Load_BindsIAuthorizationProviderToConstant()
        {
            IAuthorizationProvider expected = kernel.Get<IAuthorizationProvider>();
            IAuthorizationProvider actual = kernel.Get<IAuthorizationProvider>();

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Load_BindsIMvcSiteMapParser()
        {
            AssertBind<IMvcSiteMapParser, MvcSiteMapParser>();
        }

        [Test]
        [Ignore("Globalization provider uses virtual server path.")]
        public void Load_BindsIGlobalizationProvider()
        {
            AssertBind<IGlobalizationProvider, GlobalizationProvider>();
        }

        [Test]
        [Ignore("Site map provider uses virtual server path.")]
        public void Load_BindsIMvcSiteMapProvider()
        {
            AssertBind<IMvcSiteMapProvider, MvcSiteMapProvider>();
        }

        [Test]
        public void Load_BindsIHasher()
        {
            AssertBind<IHasher, BCrypter>();
        }

        [Test]
        public void Load_BindsIRolesService()
        {
            AssertBind<IRoleService, RoleService>();
        }

        [Test]
        public void Load_BindsIAccountsService()
        {
            AssertBind<IAccountService, AccountService>();
        }

        [Test]
        public void Load_BindsIRoleValidator()
        {
            AssertBind<IRoleValidator, RoleValidator>();
        }

        [Test]
        public void Load_BindsIAccountValidator()
        {
            AssertBind<IAccountValidator, AccountValidator>();
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
