using MvcTemplate.Components.Alerts;
using MvcTemplate.Controllers.Auth;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Validators;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Controllers.Auth
{
    [TestFixture]
    public class AuthControllerTests : AControllerTests
    {
        private AccountRecoveryView accountRecovery;
        private AccountResetView accountReset;
        private AccountLoginView accountLogin;
        private IAccountValidator validator;
        private AuthController controller;
        private IAccountService service;
        private AccountView account;

        [SetUp]
        public void SetUp()
        {
            validator = Substitute.For<IAccountValidator>();
            service = Substitute.For<IAccountService>();

            accountRecovery = ObjectFactory.CreateAccountRecoveryView();
            accountReset = ObjectFactory.CreateAccountResetView();
            accountLogin = ObjectFactory.CreateAccountLoginView();
            account = ObjectFactory.CreateAccountView();

            controller = Substitute.ForPartsOf<AuthController>(service, validator);
            controller.Url = new UrlHelper(new HttpMock().HttpContext.Request.RequestContext);
            controller.ControllerContext = new ControllerContext();
        }

        #region Method: Register()

        [Test]
        public void Register_RedirectsToDefaultlIfAlreadyLoggedIn()
        {
            service.IsLoggedIn().Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Register();

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Register_ReturnsNullModelIfNotLoggedIn()
        {
            service.IsLoggedIn().Returns(false);

            Object model = (controller.Register() as ViewResult).Model;

            Assert.IsNull(model);
        }

        #endregion

        #region Method: Register(AccountView account)

        [Test]
        public void Register_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "Register");
        }

        [Test]
        public void Register_OnPostRedirectsToDefaultlIfAlreadyLoggedIn()
        {
            service.IsLoggedIn().Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Register(null);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Register_ReturnsSameModelIfCanNotRegister()
        {
            validator.CanRegister(account).Returns(false);
            service.IsLoggedIn().Returns(false);

            Object actual = (controller.Register(account) as ViewResult).Model;
            Object expected = account;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Register_RegistersAccount()
        {
            validator.CanRegister(account).Returns(true);
            service.IsLoggedIn().Returns(false);

            controller.Register(account);

            service.Received().Register(account);
        }

        [Test]
        public void Register_AddsSuccessfulRegistrationMessage()
        {
            validator.CanRegister(account).Returns(true);
            service.IsLoggedIn().Returns(false);

            controller.Register(account);

            Alert actual = controller.Alerts.First();

            Assert.AreEqual(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.AreEqual(Messages.SuccesfulRegistration, actual.Message);
            Assert.AreEqual(AlertTypes.Success, actual.Type);
        }

        [Test]
        public void Register_RedirectsToLoginAfterSuccessfulRegistration()
        {
            validator.CanRegister(account).Returns(true);
            service.IsLoggedIn().Returns(false);

            RouteValueDictionary actual = (controller.Register(account) as RedirectToRouteResult).RouteValues;

            Assert.AreEqual("Login", actual["action"]);
            Assert.IsNull(actual["controller"]);
            Assert.IsNull(actual["language"]);
            Assert.IsNull(actual["area"]);
        }

        #endregion

        #region Method: Recover()

        [Test]
        public void Recover_RedirectsToDefaultlIfAlreadyLoggedIn()
        {
            service.IsLoggedIn().Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Recover();

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Recover_ReturnsNullModelIfNotLoggedIn()
        {
            service.IsLoggedIn().Returns(false);

            Object model = (controller.Recover() as ViewResult).Model;

            Assert.IsNull(model);
        }

        #endregion

        #region Method: Recover(AccountRecoveryView account)

        [Test]
        public void Recover_OnPostRedirectsToDefaultlIfAlreadyLoggedIn()
        {
            service.IsLoggedIn().Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Recover(null);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Recover_ReturnsSameModelIfCanNotRecover()
        {
            validator.CanRecover(accountRecovery).Returns(false);
            service.IsLoggedIn().Returns(false);

            Object actual = (controller.Recover(accountRecovery) as ViewResult).Model;
            Object expected = accountRecovery;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Recover_RecoversAccount()
        {
            validator.CanRecover(accountRecovery).Returns(true);

            controller.Recover(accountRecovery);

            service.Received().Recover(accountRecovery);
        }

        [Test]
        public void Recover_AddsRecoveryInformationMessage()
        {
            validator.CanRecover(accountRecovery).Returns(true);
            service.IsLoggedIn().Returns(false);

            controller.Recover(accountRecovery);

            Alert actual = controller.Alerts.First();

            Assert.AreEqual(Messages.RecoveryInformation, actual.Message);
            Assert.AreEqual(AlertTypes.Info, actual.Type);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        [Test]
        public void Recover_RedirectsToLoginAfterSuccessfulRecovery()
        {
            validator.CanRecover(accountRecovery).Returns(true);
            service.IsLoggedIn().Returns(false);

            RouteValueDictionary actual = (controller.Recover(accountRecovery) as RedirectToRouteResult).RouteValues;

            Assert.AreEqual("Login", actual["action"]);
            Assert.IsNull(actual["controller"]);
            Assert.IsNull(actual["language"]);
            Assert.IsNull(actual["area"]);
        }

        #endregion

        #region Method: Reset()

        [Test]
        public void Reset_RedirectsToDefaultlIfAlreadyLoggedIn()
        {
            service.IsLoggedIn().Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Reset(String.Empty);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Reset_ReturnsNullModelIfNotLoggedIn()
        {
            service.IsLoggedIn().Returns(false);

            Object model = (controller.Reset(String.Empty) as ViewResult).Model;

            Assert.IsNull(model);
        }

        #endregion

        #region Method: Reset(AccountResetView account)

        [Test]
        public void Reset_OnPostRedirectsToDefaultlIfAlreadyLoggedIn()
        {
            service.IsLoggedIn().Returns(true);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToDefault();
            ActionResult actual = controller.Reset(accountReset);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Reset_RedirectsToRecoverIfCanNotReset()
        {
            service.IsLoggedIn().Returns(false);
            validator.CanReset(accountReset).Returns(false);

            Object actual = (controller.Reset(accountReset) as RedirectToRouteResult).RouteValues["action"];
            Object expected = "Recover";

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Reset_ResetsAccount()
        {
            service.IsLoggedIn().Returns(false);
            validator.CanReset(accountReset).Returns(true);

            controller.Reset(accountReset);

            service.Received().Reset(accountReset);
        }

        [Test]
        public void Reset_AddsSuccessfulResetMessage()
        {
            service.IsLoggedIn().Returns(false);
            validator.CanReset(accountReset).Returns(true);

            controller.Reset(accountReset);

            Alert actual = controller.Alerts.First();

            Assert.AreEqual(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.AreEqual(Messages.SuccesfulReset, actual.Message);
            Assert.AreEqual(AlertTypes.Success, actual.Type);
        }

        [Test]
        public void Reset_AfterSuccesfulResetRedirectsToLogin()
        {
            service.IsLoggedIn().Returns(false);
            validator.CanReset(accountReset).Returns(true);

            RouteValueDictionary actual = (controller.Reset(accountReset) as RedirectToRouteResult).RouteValues;

            Assert.AreEqual("Login", actual["action"]);
            Assert.IsNull(actual["controller"]);
            Assert.IsNull(actual["language"]);
            Assert.IsNull(actual["area"]);
        }

        #endregion

        #region Method: Login(String returnUrl)

        [Test]
        public void Login_RedirectsToUrlIfAlreadyLoggedIn()
        {
            controller.RedirectToLocal("/Home/Index").Returns(new RedirectResult("/Home/Index"));
            service.IsLoggedIn().Returns(true);

            ActionResult expected = controller.RedirectToLocal("/Home/Index");
            ActionResult actual = controller.Login("/Home/Index");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_ReturnsNullModelIfNotLoggedIn()
        {
            service.IsLoggedIn().Returns(false);

            Object model = (controller.Login("/") as ViewResult).Model;

            Assert.IsNull(model);
        }

        #endregion

        #region Method: Login(AccountLoginView account, String returnUrl)

        [Test]
        public void Login_OnPostRedirectsToUrlIfAlreadyLoggedIn()
        {
            controller.RedirectToLocal("/Home/Index").Returns(new RedirectResult("/Home/Index"));
            service.IsLoggedIn().Returns(true);

            ActionResult expected = controller.RedirectToLocal("/Home/Index");
            ActionResult actual = controller.Login(null, "/Home/Index");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_ReturnsNullModelIfCanNotLogin()
        {
            validator.CanLogin(accountLogin).Returns(false);

            Object model = (controller.Login(accountLogin, null) as ViewResult).Model;

            Assert.IsNull(model);
        }

        [Test]
        public void Login_LogsInAccount()
        {
            validator.CanLogin(accountLogin).Returns(true);

            controller.Login(accountLogin, null);

            service.Received().Login(accountLogin.Username);
        }

        [Test]
        public void Login_RedirectsToUrlIfCanLogin()
        {
            controller.RedirectToLocal("/Home/Index").Returns(new RedirectResult("/Home/Index"));
            validator.CanLogin(accountLogin).Returns(true);

            ActionResult actual = controller.Login(accountLogin, "/Home/Index");
            ActionResult expected = controller.RedirectToLocal("/Home/Index");

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Test]
        public void Logout_LogsOut()
        {
            controller.Logout();

            service.Received().Logout();
        }

        [Test]
        public void Logout_RedirectsToLogin()
        {
            Object actual = controller.Logout().RouteValues["action"];
            Object expected = "Login";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
