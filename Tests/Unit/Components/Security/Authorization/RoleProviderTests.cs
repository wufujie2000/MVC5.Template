using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Tests.Unit.Security
{
    [TestFixture]
    public class RoleProviderTests
    {
        private RoleProvider provider;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            provider = new RoleProvider(Assembly.GetExecutingAssembly(), new UnitOfWork(context));

            TearDownData();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            provider.Dispose();
        }

        #region Method: GetAccountPrivileges(String accountId)

        [Test]
        public void GetAccountPrivileges_GetAccountsPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor("Administration", "Roles", "Index");

            IEnumerable<AccountPrivilege> actual = provider.GetAccountPrivileges(account.Id);
            IEnumerable<AccountPrivilege> expected = context.Set<Account>()
                .First(acc => acc.Id == account.Id)
                .Role
                .RolePrivileges
                .Select(role => new AccountPrivilege
                    {
                        AccountId = account.Id,

                        Area = role.Privilege.Area,
                        Controller = role.Privilege.Controller,
                        Action = role.Privilege.Action
                    });

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetAccountPrivileges_GetsNoPrivilegesForAccountWithoutRole()
        {
            IEnumerable<AccountPrivilege> actual = provider.GetAccountPrivileges(CreateAccount().Id);

            CollectionAssert.IsEmpty(actual);
        }

        [Test]
        public void GetAccountPrivileges_GetsNoPrivilegesForNotExistingAccount()
        {
            IEnumerable<AccountPrivilege> actual = provider.GetAccountPrivileges("Test");

            CollectionAssert.IsEmpty(actual);
        }

        #endregion

        #region Method: IsAuthorizedFor(String accountId, String area, String controller, String action)

        #region Not attributed controller

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "NotAttributed", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedNonGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "NotAttributed", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "NotAttributed", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnNotAttributedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedNonGetActionOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "NotAttributed", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedNonGetActionOnNotAttributedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizeNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "NotAttributed", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousNonGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "NotAttributed", "AllowAnonymousNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "NotAttributed", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedNonGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "NotAttributed", "AllowUnauthorizedNonGetAction"));
        }

        #endregion

        #region Authorized controller

        [Test]
        public void IsAuthorizedFor_NotAuthorizesNotAttributedGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "Authorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "NotAttributedGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesNotAttributedNonGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "Authorized", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedNonGetActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "NotAttributedNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedNonGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "Authorized", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedNonGetActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizeNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "Authorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousNonGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "Authorized", "AllowAnonymousNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "Authorized", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedNonGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "Authorized", "AllowUnauthorizedNonGetAction"));
        }

        #endregion

        #region Allow anonymous controller

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowAnonymous", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedNonGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowAnonymous", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "AllowAnonymous", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowAnonymous", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedNonGetActionOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "AllowAnonymous", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedNonGetActionOnAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowAnonymous", "AuthorizeNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowAnonymous", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousNonGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowAnonymous", "AllowAnonymousNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowAnonymous", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedNonGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowAnonymous", "AllowUnauthorizedNonGetAction"));
        }

        #endregion

        #region Allow unauthorized controller

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowUnauthorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedNonGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowUnauthorized", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "AllowUnauthorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowUnauthorized", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedNonGetActionOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "AllowUnauthorized", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedNonGetActionOnAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowUnauthorized", "AuthorizeNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowUnauthorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousNonGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowUnauthorized", "AllowAnonymousNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowUnauthorized", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedNonGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "AllowUnauthorized", "AllowUnauthorizedNonGetAction"));
        }

        #endregion

        #region Inherited authorized controller

        [Test]
        public void IsAuthorizedFor_NotAuthorizesGetActionOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "InheritedAuthorized", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "GetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesNonGetActionOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "InheritedAuthorized", "NonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNonGetActionOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "NonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "NonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesInheritedAllowAnonymouActionOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "InheritedAuthorized", "AllowAnonymousGetAction"));
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
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "InheritedAllowAnonymous", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNonGetActionOnInheritedAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "InheritedAllowAnonymous", "NonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesInheritedAllowAnonymouActionOnInheritedAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "InheritedAllowAnonymous", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymouActionOnInheritedAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAllowAnonymous", "AllowAnonymousGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowAnonymous", "AllowAnonymousGetAction"));
        }

        #endregion

        #region Inherited alow unauthorized controller

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionOnInheritedAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "InheritedAllowUnauthorized", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNonGetActionOnInheritedAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "InheritedAllowUnauthorized", "NonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesInheritedAllowAnonymouActionOnInheritedAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, null, "InheritedAllowUnauthorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymouActionOnInheritedAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAllowUnauthorized", "AllowAnonymousGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowUnauthorized", "AllowAnonymousGetAction"));
        }

        #endregion

        [Test]
        public void IsAuthorizedFor_OnNotExistingActionThrows()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "Test");
            String expectedMessage = "'NotAttributedController' does not have 'Test' action";

            Assert.Throws<Exception>(() => provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "Test"), expectedMessage);
        }

        #endregion

        #region Method: IsAuthorizedFor(IEnumerable<AccountPrivilege> privileges, String area, String controller, String action)

        #region Not attributed controller

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesNotAttributedGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "NotAttributed", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesNotAttributedNonGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "NotAttributed", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesAuthorizedGetActionOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "NotAttributed", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAuthorizedGetActionOnNotAttributedController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "NotAttributed", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "NotAttributed", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesAuthorizedNonGetActionOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "NotAttributed", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAuthorizedNonGetActionOnNotAttributedController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "NotAttributed", "AuthorizeNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "NotAttributed", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowAnonymousGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "NotAttributed", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowAnonymousNonGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "NotAttributed", "AllowAnonymousNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowUnauthorizedGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "NotAttributed", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowUnauthorizedNonGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "NotAttributed", "AllowUnauthorizedNonGetAction"));
        }

        #endregion

        #region Authorized controller

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesNotAttributedGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Authorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesNotAttributedGetActionOnAuthorizedController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "Authorized", "NotAttributedGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "Authorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesNotAttributedNonGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Authorized", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesNotAttributedNonGetActionOnAuthorizedController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "Authorized", "NotAttributedNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "Authorized", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesAuthorizedGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAuthorizedGetActionOnAuthorizedController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "Authorized", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesAuthorizedNonGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Authorized", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAuthorizedNonGetActionOnAuthorizedController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "Authorized", "AuthorizeNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "Authorized", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowAnonymousGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Authorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowAnonymousNonGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Authorized", "AllowAnonymousNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowUnauthorizedGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Authorized", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowUnauthorizedNonGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Authorized", "AllowUnauthorizedNonGetAction"));
        }

        #endregion

        #region Allow anonymous controller

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesNotAttributedGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowAnonymous", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesNotAttributedNonGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowAnonymous", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesAuthorizedGetActionOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowAnonymous", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAuthorizedGetActionOnAllowAnonymousController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "AllowAnonymous", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "AllowAnonymous", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesAuthorizedNonGetActionOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowAnonymous", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAuthorizedNonGetActionOnAllowAnonymousController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "AllowAnonymous", "AuthorizeNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "AllowAnonymous", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowAnonymousGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowAnonymous", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowAnonymousNonGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowAnonymous", "AllowAnonymousNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowUnauthorizedGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowAnonymous", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowUnauthorizedNonGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowAnonymous", "AllowUnauthorizedNonGetAction"));
        }

        #endregion

        #region Allow unauthorized controller

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesNotAttributedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowUnauthorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesNotAttributedNonGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowUnauthorized", "NotAttributedNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesAuthorizedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowUnauthorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAuthorizedGetActionOnAllowUnauthorizedController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "AllowUnauthorized", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "AllowUnauthorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_NotAuthorizesAuthorizedNonGetActionOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowUnauthorized", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAuthorizedNonGetActionOnAllowUnauthorizedController()
        {
            IEnumerable<AccountPrivilege> privileges = CreateAccountPrivilegeEnumerableFor(null, "AllowUnauthorized", "AuthorizeNonGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, null, "AllowUnauthorized", "AuthorizeNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowAnonymousGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowUnauthorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowAnonymousNonGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowUnauthorized", "AllowAnonymousNonGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowUnauthorizedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowUnauthorized", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_WithPrivileges_AuthorizesAllowUnauthorizedNonGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "AllowUnauthorized", "AllowUnauthorizedNonGetAction"));
        }

        #endregion

        [Test]
        public void IsAuthorizedFor_WithPrivileges_OnNotExistingActionThrows()
        {
            String expectedMessage = "'NotAttributedController' does not have 'Test' action";

            Assert.Throws<Exception>(() => provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "NotAttributed", "Test"), expectedMessage);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesUnitOfWork()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            new RoleProvider(Assembly.GetExecutingAssembly(), unitOfWork).Dispose();

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

        private Account CreateAccount()
        {
            Account account = ObjectFactory.CreateAccount();
            context.Set<Account>().Add(account);
            context.SaveChanges();

            return account;
        }
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

            return account;
        }
        private IEnumerable<AccountPrivilege> CreateAccountPrivilegeEnumerableFor(String area, String controller, String action)
        {
            return new List<AccountPrivilege>()
            {
                new AccountPrivilege()
                {
                    Area = area,
                    Controller = controller,
                    Action = action
                }
            };
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
