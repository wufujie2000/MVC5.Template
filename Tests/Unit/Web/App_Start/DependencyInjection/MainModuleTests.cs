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
        [TestCase(typeof(IGlobalizationProvider), typeof(GlobalizationProvider), IgnoreReason = "Globalization provider uses virtual server path.")]
        [TestCase(typeof(IMvcSiteMapProvider), typeof(MvcSiteMapProvider), IgnoreReason = "Site map provider uses virtual server path.")]
        
        [TestCase(typeof(IRoleService), typeof(RoleService))]
        [TestCase(typeof(IAccountService), typeof(AccountService))]
        
        [TestCase(typeof(IRoleValidator), typeof(RoleValidator))]
        [TestCase(typeof(IAccountValidator), typeof(AccountValidator))]
        public void Load_BindsToImplementation(Type abstraction, Type implementation)
        {
            Type actual = kernel.Get(abstraction).GetType();
            Type expected = implementation;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(typeof(IAuthorizationProvider), typeof(AuthorizationProvider))]
        public void Load_BindsToConstantImplementation(Type abstraction, Type implementation)
        {
            IAuthorizationProvider expected = kernel.Get<IAuthorizationProvider>();
            IAuthorizationProvider actual = kernel.Get<IAuthorizationProvider>();

            Assert.AreEqual(actual.GetType(), implementation);
            Assert.AreSame(expected, actual);
        }

        #endregion
    }
}
