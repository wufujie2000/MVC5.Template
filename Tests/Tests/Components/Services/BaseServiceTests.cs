using NUnit.Framework;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Tests.Objects.Components.Services;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class BaseServiceTests
    {
        private BaseServiceStub service;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContextStub().Context;
            service = new BaseServiceStub();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            service.Dispose();
            service.Dispose();
        }

        #region Property: CurrentLanguage

        [Test]
        public void CurrentLanguage_IsCurrent()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["language"] = "te-TT";
            Assert.AreEqual("te-TT", service.CurrentLanguage);
        }

        #endregion

        #region Property: CurrentArea

        [Test]
        public void CurrentArea_IsCurrent()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["area"] = "TestArea";
            Assert.AreEqual("TestArea", service.CurrentArea);
        }

        #endregion

        #region Property: CurrentController

        [Test]
        public void CurrentController_IsCurrent()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] = "TestControlller";
            Assert.AreEqual("TestControlller", service.CurrentController);
        }

        #endregion

        #region Property: CurrentAction

        [Test]
        public void CurrentAction_IsCurrent()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["action"] = "TestAction";
            Assert.AreEqual("TestAction", service.CurrentAction);
        }

        #endregion

        #region Property: CurrentAccountId

        [Test]
        public void CurrentAccountId_IsCurrent()
        {
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("TestUser"), new String[0]);
            Assert.AreEqual("TestUser", service.CurrentAccountId);
        }

        #endregion

        #region Constructor: BaseService() : this(null)

        [Test]
        public void BaseService_HasNotNullUnitOfWork()
        {
            Assert.IsNotNull(new BaseServiceStub().BaseUnitOfWork);
        }

        [Test]
        public void BaseService_AlertMessagesIsInstanceOfMessagesContainer()
        {
            Assert.AreEqual(typeof(MessagesContainer), new BaseServiceStub().AlertMessages.GetType());
        }

        #endregion

        #region Constructor: BaseService(ModelStateDictionary modelState)

        [Test]
        public void BaseService_MessagesContainerContainsError()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("TestKey", "TestError");
            
            var service = new BaseServiceStub(modelState);

            Assert.AreEqual(1, service.AlertMessages.Count());
            Assert.AreEqual("TestKey", service.AlertMessages.First().Key);
            Assert.AreEqual("TestError", service.AlertMessages.First().Message);
        }

        #endregion
    }
}
