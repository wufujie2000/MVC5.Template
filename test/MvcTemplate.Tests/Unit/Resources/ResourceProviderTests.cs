using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Tests.Objects;
using System;
using System.Web.Routing;
using Xunit;

namespace MvcTemplate.Tests.Unit.Resources
{
    public class ResourceProviderTests
    {
        #region Static method: GetDatalistTitle<TModel>()

        [Fact]
        public void GetDatalistTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Datalist.Titles.Role;
            String actual = ResourceProvider.GetDatalistTitle<Role>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDatalistTitle_OnTypeNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetDatalistTitle<Object>());
        }

        #endregion

        #region Static method: GetContentTitle(RouteValueDictionary values)

        [Fact]
        public void GetContentTitle_GetsTitle()
        {
            RouteValueDictionary values = new RouteValueDictionary();
            values["area"] = "Administration";
            values["controller"] = "Accounts";
            values["action"] = "Details";

            String expected = ContentTitles.AdministrationAccountsDetails;
            String actual = ResourceProvider.GetContentTitle(values);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetContentTitle_GetsTitleWithoutArea()
        {
            RouteValueDictionary values = new RouteValueDictionary();
            values["controller"] = "Profile";
            values["action"] = "Edit";
            values["area"] = null;

            String actual = ResourceProvider.GetContentTitle(values);
            String expected = ContentTitles.ProfileEdit;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetContentTitle_OnTitleNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetContentTitle(new RouteValueDictionary()));
        }

        #endregion

        #region Static method: GetSiteMapTitle(String area, String controller, String action)

        [Fact]
        public void GetMenuTitle_GetsTitle()
        {
            String actual = ResourceProvider.GetSiteMapTitle("Administration", "Roles", "Index");
            String expected = MvcTemplate.Resources.SiteMap.Titles.AdministrationRolesIndex;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMenuTitle_GetsTitleWithoutControllerAndAction()
        {
            String actual = ResourceProvider.GetSiteMapTitle("Administration", null, null);
            String expected = MvcTemplate.Resources.SiteMap.Titles.Administration;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMenuTitle_OnMenuNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetSiteMapTitle("Test", "Test", "Test"));
        }

        #endregion

        #region Static method: GetPrivilegeAreaTitle(String area)

        [Fact]
        public void GetPrivilegeAreaTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Area.Titles.Administration;
            String actual = ResourceProvider.GetPrivilegeAreaTitle("Administration");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeAreaTitle_OnActionNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeAreaTitle("Test"));
        }

        #endregion

        #region Static method: GetPrivilegeControllerTitle(String area, String controller)

        [Fact]
        public void GetPrivilegeControllerTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Controller.Titles.AdministrationRoles;
            String actual = ResourceProvider.GetPrivilegeControllerTitle("Administration", "Roles");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeControllerTitle_OnActionNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeControllerTitle("", ""));
        }

        #endregion

        #region Static method: GetPrivilegeActionTitle(String area, String controller, String action)

        [Fact]
        public void GetPrivilegeActionTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Action.Titles.AdministrationAccountsIndex;
            String actual = ResourceProvider.GetPrivilegeActionTitle("Administration", "Accounts", "Index");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeActionTitle_OnActionNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeActionTitle("", "", ""));
        }

        #endregion

        #region Static method: GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)

        [Fact]
        public void GetPropertyTitle_OnNotMemberExpressionReturnNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<TestView, String>(view => view.ToString()));
        }

        [Fact]
        public void GetPropertyTitle_GetsTitleFromExpression()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Username);
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_GetsTitleFromExpressionRelation()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.RoleName);
            String expected = MvcTemplate.Resources.Views.RoleView.Titles.Id;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_OnPropertyFromExpressionNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Id));
        }

        [Fact]
        public void GetPropertyTitle_OnTypeFromExpressionNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<TestView, String>(test => test.Text));
        }

        #endregion

        #region Static method: GetPropertyTitle(Type view, String property)

        [Fact]
        public void GetPropertyTitle_GetsTitle()
        {
            String actual = ResourceProvider.GetPropertyTitle(typeof(AccountView), "Username");
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_GetsTitleFromRelation()
        {
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountUsername");
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_GetsTitleFromMultipleRelations()
        {
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountRoleAccountUsername");
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_OnPropertyNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(AccountView), "Id"));
        }

        [Fact]
        public void GetPropertyTitle_OnTypeNotFoundReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(TestView), "Title"));
        }

        #endregion
    }
}
