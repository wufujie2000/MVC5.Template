using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Resources
{
    [TestFixture]
    public class ResourceProviderTests
    {
        #region Static method: GetDatalistTitle<TModel>()

        [Test]
        public void GetDatalistTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Datalist.Titles.Role;
            String actual = ResourceProvider.GetDatalistTitle<Role>();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetDatalistTitle_OnTypeNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetDatalistTitle<Object>());
        }

        #endregion

        #region Static method: GetActionTitle(String action)

        [Test]
        public void GetActionTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Action.Titles.Create;
            String actual = ResourceProvider.GetActionTitle("Create");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetActionTitle_OnActionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetActionTitle(null));
        }

        #endregion

        #region Static method: GetContentTitle(RouteValueDictionary values)

        [Test]
        public void GetContentTitle_GetsTitle()
        {
            RouteValueDictionary values = new RouteValueDictionary();
            values["area"] = "Administration";
            values["controller"] = "Accounts";
            values["action"] = "Details";

            String expected = ContentTitles.AdministrationAccountsDetails;
            String actual = ResourceProvider.GetContentTitle(values);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetContentTitle_GetsTitleWithoutArea()
        {
            RouteValueDictionary values = new RouteValueDictionary();
            values["controller"] = "Profile";
            values["action"] = "Edit";
            values["area"] = null;

            String actual = ResourceProvider.GetContentTitle(values);
            String expected = ContentTitles.ProfileEdit;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetContentTitle_OnTitleNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetContentTitle(new RouteValueDictionary()));
        }

        #endregion

        #region Static method: GetSiteMapTitle(String area, String controller, String action)

        [Test]
        public void GetMenuTitle_GetsTitle()
        {
            String actual = ResourceProvider.GetSiteMapTitle("Administration", "Roles", "Index");
            String expected = MvcTemplate.Resources.SiteMap.Titles.AdministrationRolesIndex;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetMenuTitle_GetsTitleWithoutControllerAndAction()
        {
            String actual = ResourceProvider.GetSiteMapTitle("Administration", null, null);
            String expected = MvcTemplate.Resources.SiteMap.Titles.Administration;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetMenuTitle_OnMenuNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetSiteMapTitle("Test", "Test", "Test"));
        }

        #endregion

        #region Static method: GetPrivilegeAreaTitle(String area)

        [Test]
        public void GetPrivilegeAreaTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Area.Titles.Administration;
            String actual = ResourceProvider.GetPrivilegeAreaTitle("Administration");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPrivilegeAreaTitle_OnActionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPrivilegeAreaTitle("Test"));
        }

        #endregion

        #region Static method: GetPrivilegeControllerTitle(String area, String controller)

        [Test]
        public void GetPrivilegeControllerTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Controller.Titles.AdministrationRoles;
            String actual = ResourceProvider.GetPrivilegeControllerTitle("Administration", "Roles");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPrivilegeControllerTitle_OnActionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPrivilegeControllerTitle("", ""));
        }

        #endregion

        #region Static method: GetPrivilegeActionTitle(String area, String controller, String action)

        [Test]
        public void GetPrivilegeActionTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Action.Titles.AdministrationAccountsIndex;
            String actual = ResourceProvider.GetPrivilegeActionTitle("Administration", "Accounts", "Index");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPrivilegeActionTitle_OnActionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPrivilegeActionTitle("", "", ""));
        }

        #endregion

        #region Static method: GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)

        [Test]
        public void GetPropertyTitle_OnNotMemberExpressionThrows()
        {
            Exception expected = Assert.Throws<InvalidOperationException>(() => ResourceProvider.GetPropertyTitle<TestView, String>(view => view.ToString()));
            Assert.AreEqual(expected.Message, "Expression must be a member expression.");
        }

        [Test]
        public void GetPropertyTitle_GetsTitleFromExpression()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Username);
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPropertyTitle_GetsTitleFromExpressionRelation()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.RoleId);
            String expected = MvcTemplate.Resources.Views.RoleView.Titles.Id;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPropertyTitle_OnPropertyFromExpressionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Id));
        }

        [Test]
        public void GetPropertyTitle_OnTypeFromExpressionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPropertyTitle<TestView, String>(test => test.Text));
        }

        #endregion

        #region Static method: GetPropertyTitle(Type view, String property)

        [Test]
        public void GetPropertyTitle_GetsTitle()
        {
            String actual = ResourceProvider.GetPropertyTitle(typeof(AccountView), "Username");
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPropertyTitle_GetsTitleFromRelation()
        {
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountUsername");
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPropertyTitle_GetsTitleFromMultipleRelations()
        {
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountRoleAccountUsername");
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPropertyTitle_OnPropertyNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPropertyTitle(typeof(AccountView), "Id"));
        }

        [Test]
        public void GetPropertyTitle_OnTypeNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPropertyTitle(typeof(TestView), "Title"));
        }

        #endregion
    }
}
