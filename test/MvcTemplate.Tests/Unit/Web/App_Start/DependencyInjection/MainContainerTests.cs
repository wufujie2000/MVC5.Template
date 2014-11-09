using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using MvcTemplate.Web;
using MvcTemplate.Web.DependencyInjection;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Web.DependencyInjection
{
    [TestFixture]
    public class MainContainerTests
    {
        private MainContainer container;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            container = new MainContainer();
            container.RegisterServices();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            container.Dispose();
        }

        #region Method: RegisterServices()

        [Test]
        [TestCase(typeof(ILogger), typeof(Logger))]
        [TestCase(typeof(AContext), typeof(Context))]
        [TestCase(typeof(IUnitOfWork), typeof(UnitOfWork))]
        [TestCase(typeof(IAuditLogger), typeof(AuditLogger))]

        [TestCase(typeof(IHasher), typeof(BCrypter))]
        [TestCase(typeof(IMailClient), typeof(SmtpMailClient))]

        [TestCase(typeof(IRouteConfig), typeof(RouteConfig))]
        [TestCase(typeof(IBundleConfig), typeof(BundleConfig))]
        [TestCase(typeof(IExceptionFilter), typeof(ExceptionFilter))]

        [TestCase(typeof(IMvcSiteMapParser), typeof(MvcSiteMapParser))]
        [TestCase(typeof(IMvcSiteMapProvider), typeof(MvcSiteMapProvider), IgnoreReason = "Site map provider uses virtual server path.")]

        [TestCase(typeof(IGlobalizationProvider), typeof(GlobalizationProvider), IgnoreReason = "Globalization provider uses virtual server path.")]

        [TestCase(typeof(IRoleService), typeof(RoleService))]
        [TestCase(typeof(IAccountService), typeof(AccountService))]

        [TestCase(typeof(IRoleValidator), typeof(RoleValidator))]
        [TestCase(typeof(IAccountValidator), typeof(AccountValidator))]
        public void RegisterServices_RegistersTransientImplementation(Type abstraction, Type expectedType)
        {
            Object expected = container.GetInstance(abstraction);
            Object actual = container.GetInstance(abstraction);

            Assert.AreEqual(expectedType, actual.GetType());
            Assert.AreNotSame(expected, actual);
        }

        [Test]
        [TestCase(typeof(IAuthorizationProvider), typeof(AuthorizationProvider))]
        public void RegisterServices_RegistersSingletonImplementation(Type abstraction, Type expectedType)
        {
            IAuthorizationProvider expected = container.GetInstance<IAuthorizationProvider>();
            IAuthorizationProvider actual = container.GetInstance<IAuthorizationProvider>();

            Assert.AreEqual(expectedType, actual.GetType());
            Assert.AreSame(expected, actual);
        }

        #endregion
    }
}
