using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MvcTemplate.Tests.Unit.Security
{
    [TestFixture]
    public class AuthorizationProviderTests
    {
        private AuthorizationProvider provider;
        private TestingContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            provider = new AuthorizationProvider(Assembly.GetExecutingAssembly(), new UnitOfWork(context));

            TearDownData();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            provider.Dispose();
        }

        #region Method: IsAuthorizedFor(String accountId, String area, String controller, String action)

        #region Not attributed controller

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionNameOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "NotAttributedGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionNameOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "NotAttributedPostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnNotAttributedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionNameOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AuthorizeGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionNameOnNotAttributedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizeGetActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizeGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionOnNotAttributedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizePostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionNameOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AuthorizePostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionNameOnNotAttributedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizePostActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizePostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AllowAnonymousPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionNameOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AllowAnonymousGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionNameOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AllowAnonymousPostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AllowUnauthorizedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionNameOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AllowUnauthorizedGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionNameOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "NotAttributed", "AllowUnauthorizedPostActionName"));
        }

        #endregion

        #region Authorized controller

        [Test]
        public void IsAuthorizedFor_NotAuthorizesNotAttributedGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "Authorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "NotAttributedGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesNotAttributedGetActionNameOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "Authorized", "NotAttributedGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionNameOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "NotAttributedGetActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "NotAttributedGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesNotAttributedPostActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "Authorized", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "NotAttributedPostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesNotAttributedPostActionNameOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "Authorized", "NotAttributedPostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionNameOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "NotAttributedPostActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "NotAttributedPostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionNameOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "Authorized", "AuthorizeGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionNameOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizeGetActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizeGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "Authorized", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizePostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionNameOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "Authorized", "AuthorizePostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionNameOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizePostActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizePostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "Authorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "Authorized", "AllowAnonymousPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionNameOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "Authorized", "AllowAnonymousGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionNameOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "Authorized", "AllowAnonymousPostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "Authorized", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "Authorized", "AllowUnauthorizedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionNameOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "Authorized", "AllowUnauthorizedGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionNameOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "Authorized", "AllowUnauthorizedPostActionName"));
        }

        #endregion

        #region Allow anonymous controller

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionNameOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "NotAttributedGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionNameOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "NotAttributedPostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowAnonymous", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionNameOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AuthorizeGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionNameOnAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowAnonymous", "AuthorizeGetActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizeGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionOnAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowAnonymous", "AuthorizePostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionNameOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AuthorizePostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionNameOnAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowAnonymous", "AuthorizePostActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizePostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AllowAnonymousPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionNameOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AllowAnonymousGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionNameOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AllowAnonymousPostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AllowUnauthorizedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionNameOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AllowUnauthorizedGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionNameOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowAnonymous", "AllowUnauthorizedPostActionName"));
        }

        #endregion

        #region Allow unauthorized controller

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionNameOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "NotAttributedGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionNameOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "NotAttributedPostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowUnauthorized", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionNameOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AuthorizeGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionNameOnAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowUnauthorized", "AuthorizeGetActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "AuthorizeGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionOnAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowUnauthorized", "AuthorizePostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionNameOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AuthorizePostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionNameOnAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowUnauthorized", "AuthorizePostActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "AuthorizePostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AllowAnonymousPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionNameOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AllowAnonymousGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionNameOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AllowAnonymousPostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AllowUnauthorizedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionNameOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AllowUnauthorizedGetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionNameOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "AllowUnauthorized", "AllowUnauthorizedPostActionName"));
        }

        #endregion

        #region Inherited authorized controller

        [Test]
        public void IsAuthorizedFor_NotAuthorizesGetActionOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "InheritedAuthorized", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "GetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesGetActionNameOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "InheritedAuthorized", "GetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionNameOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "GetActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "GetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesPostActionOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "InheritedAuthorized", "PostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesPostActionOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "PostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "PostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesPostActionNameOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "InheritedAuthorized", "PostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesPostActionNameOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "PostActionName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "PostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesInheritedAllowAnonymouActionOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "InheritedAuthorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymouActionOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "AllowAnonymousGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "AllowAnonymousGetAction"));
        }

        #endregion

        #region Inherited allow anonymous controller

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionOnInheritedAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "InheritedAllowAnonymous", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesPostActionOnInheritedAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "InheritedAllowAnonymous", "PostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionNameOnInheritedAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "InheritedAllowAnonymous", "GetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesPostActionNameOnInheritedAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "InheritedAllowAnonymous", "PostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesInheritedAllowAnonymouActionOnInheritedAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "InheritedAllowAnonymous", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymouActionOnInheritedAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAllowAnonymous", "AllowAnonymousGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowAnonymous", "AllowAnonymousGetAction"));
        }

        #endregion

        #region Inherited allow unauthorized controller

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionOnInheritedAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "InheritedAllowUnauthorized", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesPostActionOnInheritedAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "InheritedAllowUnauthorized", "PostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionNameOnInheritedAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "InheritedAllowUnauthorized", "GetActionName"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesPostActionNameOnInheritedAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", "InheritedAllowUnauthorized", "PostActionName"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesInheritedAllowAnonymouActionOnInheritedAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", "InheritedAllowUnauthorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymouActionOnInheritedAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAllowUnauthorized", "AllowAnonymousGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowUnauthorized", "AllowAnonymousGetAction"));
        }

        #endregion

        [Test]
        public void IsAuthorizedFor_CachesAccountPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizeGetAction");
            TearDownData();

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_IgnoresCase()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "notattributed", "AUTHORIZEGETACTION"));
        }

        [Test]
        public void IsAuthorizedFor_OnNotExistingActionThrows()
        {
            Exception expected = Assert.Throws<Exception>(() => provider.IsAuthorizedFor(null, "Security", "NotAttributed", "Test"));
            Assert.AreEqual(expected.Message, "'NotAttributedController' does not have 'Test' action.");
        }

        #endregion

        #region Method: Refresh()

        [Test]
        public void Refresh_RefreshesPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizeGetAction");

            TearDownData();
            provider.Refresh();

            Assert.IsFalse(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizeGetAction"));
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesUnitOfWork()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            unitOfWork.Repository<Account>().Returns(new Repository<Account>(context));

            new AuthorizationProvider(Assembly.GetExecutingAssembly(), unitOfWork).Dispose();

            unitOfWork.Received().Dispose();
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            provider.Dispose();
            provider.Dispose();
        }

        #endregion

        #region Test helpers

        private Account CreateAccountWithPrivilegeFor(String area, String controller, String action)
        {
            Account account = ObjectFactory.CreateAccount();
            Role role = ObjectFactory.CreateRole();
            account.RoleId = role.Id;

            context.Set<Account>().Add(account);

            role.RolePrivileges = new List<RolePrivilege>();
            RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege();
            Privilege privilege = ObjectFactory.CreatePrivilege();
            rolePrivilege.PrivilegeId = privilege.Id;
            rolePrivilege.Privilege = privilege;
            rolePrivilege.RoleId = role.Id;
            rolePrivilege.Role = role;

            privilege.Area = area;
            privilege.Controller = controller;
            privilege.Action = action;

            role.RolePrivileges.Add(rolePrivilege);

            context.Set<Role>().Add(role);
            context.SaveChanges();
            provider.Refresh();

            return account;
        }

        private void TearDownData()
        {
            context.Set<Privilege>().RemoveRange(context.Set<Privilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        #endregion
    }
}
