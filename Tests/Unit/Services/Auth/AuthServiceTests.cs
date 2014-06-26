using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Template.Components.Alerts;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.AccountView;
using Template.Services;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IHasher> hasherMock;
        private AuthService service;
        private HttpMock httpMock;
        private AContext context;
        private IHasher hasher;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            hasherMock = new Mock<IHasher>(MockBehavior.Strict);
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(true);
            hasherMock.Setup(mock => mock.HashPassword(It.IsAny<String>())).Returns("Hashed");
            service = new AuthService(new UnitOfWork(context), hasherMock.Object);
            service.AlertMessages = new MessagesContainer();
            service.ModelState = new ModelStateDictionary();
            hasher = hasherMock.Object;

            httpMock = new HttpMock();
            HttpContext.Current = httpMock.HttpContext;

            TearDownData();
            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;

            service.Dispose();
            context.Dispose();
        }

        #region Method: IsLoggedIn()

        [Test]
        public void IsLoggedIn_ReturnsTrueThenAccountIsAuthenticated()
        {
            httpMock.IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(true);

            Assert.IsTrue(service.IsLoggedIn());
        }

        [Test]
        public void IsLoggedIn_ReturnsFalseThenAccountIsNotAuthenticated()
        {
            httpMock.IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(false);

            Assert.IsFalse(service.IsLoggedIn());
        }

        #endregion

        #region Method: CanLogin(AuthView account)

        [Test]
        public void CanLogin_CanNotLoginWithInvalidModelState()
        {
            service.ModelState.AddModelError("Key", "ErrorMesages");

            Assert.IsFalse(service.CanLogin(ObjectFactory.CreateAuthView()));
        }

        [Test]
        public void CanLogin_CanNotLoginFromNonExistingAccount()
        {
            AuthView account = new AuthView();
            account.Username = String.Empty;

            Assert.IsFalse(service.CanLogin(account));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithNotExistingAccount()
        {
            AuthView account = new AuthView();
            account.Username = String.Empty;
            service.CanLogin(account);

            String expected = Validations.IncorrectUsernameOrPassword;
            AlertMessage actualMessage = service.AlertMessages.First();

            Assert.AreEqual(AlertMessageType.Danger, actualMessage.Type);
            Assert.AreEqual(expected, actualMessage.Message);
        }

        [Test]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AuthView account = ObjectFactory.CreateAuthView();

            Assert.IsFalse(service.CanLogin(account));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AuthView account = ObjectFactory.CreateAuthView();
            account.Password += "Incorrect";
            service.CanLogin(account);

            String expected = Validations.IncorrectUsernameOrPassword;
            AlertMessage actualMessage = service.AlertMessages.First();

            Assert.AreEqual(AlertMessageType.Danger, actualMessage.Type);
            Assert.AreEqual(expected, actualMessage.Message);
        }

        [Test]
        public void CanLogin_CanLoginWithCaseInsensitiveUsername()
        {
            AuthView account = ObjectFactory.CreateAuthView();
            account.Username = account.Username.ToUpper();

            Assert.IsTrue(service.CanLogin(account));
        }

        [Test]
        public void CanLogin_CanLoginWithValidAccount()
        {
            Assert.IsTrue(service.CanLogin(ObjectFactory.CreateAuthView()));
        }

        #endregion

        #region Method: CanRegister(AuthView account)

        [Test]
        public void CanRegister_CanNotRegisterWithInvalidModelState()
        {
            service.ModelState.AddModelError("Key", "Error");

            Assert.IsFalse(service.CanRegister(ObjectFactory.CreateAuthView()));
        }

        [Test]
        public void CanRegister_CanNotRegisterWithAlreadyTakenUsername()
        {
            AuthView account = ObjectFactory.CreateAuthView();
            account.Username = account.Username.ToLower();

            Assert.IsFalse(service.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErorrMessageThenCanNotRegisterWithAlreadyTakenUsername()
        {
            AuthView account = ObjectFactory.CreateAuthView();
            account.Username = account.Username.ToLower();
            service.CanRegister(account);

            AlertMessage actual = service.AlertMessages.First();

            Assert.AreEqual(Validations.UsernameIsAlreadyTaken, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
        }

        [Test]
        public void CanRegister_CanNotRegisterIfPasswordIsTooShort()
        {
            AuthView account = ObjectFactory.CreateAuthView();
            account.Password = "AaaAaa1";

            Assert.IsFalse(service.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErorrMessageThenCanNotCreateIfPasswordIsTooShort()
        {
            AuthView account = ObjectFactory.CreateAuthView(2);
            account.Password = "AaaAaa1";

            service.CanRegister(account);

            AlertMessage actual = service.AlertMessages.First();

            Assert.AreEqual(Validations.IllegalPassword, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
        }

        [Test]
        public void CanRegister_CanNotCreateIfPasswordDoesNotContainUpperLetter()
        {
            AuthView account = ObjectFactory.CreateAuthView();
            account.Password = "aaaaaaaaaaaa1";

            Assert.IsFalse(service.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErorrMessageThenCanNotCreateIfPasswordDoesNotContainUpperLetter()
        {
            AuthView account = ObjectFactory.CreateAuthView(2);
            account.Password = "aaaaaaaaaaaa1";

            service.CanRegister(account);

            AlertMessage actual = service.AlertMessages.First();

            Assert.AreEqual(Validations.IllegalPassword, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
        }

        [Test]
        public void CanRegister_CanNotCreateIfPasswordDoesNotContainLowerLetter()
        {
            AuthView account = ObjectFactory.CreateAuthView(2);
            account.Password = "AAAAAAAAAAA1";

            Assert.IsFalse(service.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErorrMessageThenCanNotRegisterIfPasswordDoesNotContainLowerLetter()
        {
            AuthView account = ObjectFactory.CreateAuthView(2);
            account.Password = "AAAAAAAAAAA1";

            service.CanRegister(account);

            AlertMessage actual = service.AlertMessages.First();

            Assert.AreEqual(Validations.IllegalPassword, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
        }

        [Test]
        public void CanRegister_CanNotRegisterIfPasswordDoesNotContainADigit()
        {
            AuthView account = ObjectFactory.CreateAuthView();
            account.Password = "AaAaAaAaAaAa";

            Assert.IsFalse(service.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErorrMessageThenCanNotRegisterIfPasswordDoesNotContainADigit()
        {
            AuthView account = ObjectFactory.CreateAuthView(2);
            account.Password = "AaAaAaAaAaAa";

            service.CanRegister(account);

            AlertMessage actual = service.AlertMessages.First();

            Assert.AreEqual(Validations.IllegalPassword, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
        }

        [Test]
        public void CanRegister_CanNotRegisterWithoutEmail()
        {
            AuthView account = ObjectFactory.CreateAuthView();
            account.Email = null;

            Assert.IsFalse(service.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddEmptyModelStateErrorThenCanNotRegisterWithoutEmail()
        {
            AuthView account = ObjectFactory.CreateAuthView();
            account.Email = null;

            service.CanRegister(account);

            String actual = service.ModelState["Email"].Errors[0].ErrorMessage;
            String expected = String.Empty;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanRegister_CanNotRegisterWithAlreadyUsedEmail()
        {
            AuthView account = ObjectFactory.CreateAuthView();

            Assert.IsFalse(service.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErrorMessageThenCanNotRegisterWithAlreadyUsedEmail()
        {
            AuthView account = ObjectFactory.CreateAuthView();
            account.Username += "Test username";
            service.CanRegister(account);

            AlertMessage actual = service.AlertMessages.First();

            Assert.AreEqual(Validations.EmailIsAlreadyUsed, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
        }

        [Test]
        public void CanRegister_CanRegisterValidAccount()
        {
            Assert.IsTrue(service.CanRegister(ObjectFactory.CreateAuthView(2)));
        }

        #endregion

        #region Method: Register(AuthView account)

        [Test]
        public void Register_CreatesAccount()
        {
            TearDownData();

            AuthView expected = ObjectFactory.CreateAuthView();
            service.Register(expected);

            Account actual = context.Set<Account>().SingleOrDefault(account => account.Id == expected.Id);

            Assert.AreEqual(hasher.HashPassword(expected.Password), actual.Passhash);
            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.RoleId);
        }

        #endregion

        #region Method: Login(AuthView account)

        [Test]
        public void Login_SetsAccountId()
        {
            AuthView accountView = ObjectFactory.CreateAuthView();
            String expected = accountView.Id;
            accountView.Id = null;

            service.Login(accountView);
            String actual = accountView.Id;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_CreatesCookieForAMonth()
        {
            AuthView accountView = ObjectFactory.CreateAuthView();
            service.Login(accountView);

            DateTime actual = HttpContext.Current.Response.Cookies[0].Expires.Date;
            DateTime expected = DateTime.Now.AddMonths(1).Date;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_CreatesPersistentCookie()
        {
            AuthView authView = ObjectFactory.CreateAuthView();
            service.Login(authView);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            Assert.IsTrue(ticket.IsPersistent);
        }

        [Test]
        public void Login_CreatesCookieWithoutClientSideAccess()
        {
            AuthView authView = ObjectFactory.CreateAuthView();
            service.Login(authView);

            Assert.IsTrue(HttpContext.Current.Response.Cookies[0].HttpOnly);
        }

        [Test]
        public void Login_SetAccountIdAsCookieValue()
        {
            AuthView authView = ObjectFactory.CreateAuthView();
            service.Login(authView);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);
            String expected = authView.Id;
            String actual = ticket.Name;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Test]
        public void Logout_MakesAccountCookieExpired()
        {
            AuthView authView = ObjectFactory.CreateAuthView();
            service.Login(authView);
            service.Logout();

            DateTime cookieExpirationDate = HttpContext.Current.Response.Cookies[0].Expires;

            Assert.That(cookieExpirationDate, Is.LessThan(DateTime.Now));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            Account account = ObjectFactory.CreateAccount();

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<Account>().RemoveRange(context.Set<Account>().Where(account => account.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}
