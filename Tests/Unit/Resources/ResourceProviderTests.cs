using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System;
using System.Web;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Resources
{
    [TestFixture]
    public class ResourceProviderTests
    {
        private RouteValueDictionary routeValues;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpMock().HttpContext;
            routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Static method: GetCurrentFormTitle()

        [Test]
        public void GetCurrentFormTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Form.Titles.AdministrationAccounts;
            String actual = ResourceProvider.GetCurrentFormTitle();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCurrentFormTitle_GetsTitleWithoutArea()
        {
            routeValues["controller"] = "Profile";
            routeValues["area"] = null;

            String expected = MvcTemplate.Resources.Form.Titles.Profile;
            String actual = ResourceProvider.GetCurrentFormTitle();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCurrentFormTitle_OnFormNotFoundReturnsNull()
        {
            routeValues["controller"] = "Controller";

            Assert.IsNull(ResourceProvider.GetCurrentFormTitle());
        }

        #endregion

        #region Static method: GetCurrentTableTitle()

        [Test]
        public void GetCurrentTableTitle_GetsTitle()
        {
            routeValues["action"] = "Index";

            String expected = MvcTemplate.Resources.Table.Titles.AdministrationAccountsIndex;
            String actual = ResourceProvider.GetCurrentTableTitle();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Ignore]
        public void GetCurrentTableTitle_GetsTitleWithoutArea()
        {
            Assert.Inconclusive("No table without area");
        }

        [Test]
        public void GetCurrentTableTitle_OnTableNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetCurrentTableTitle());
        }

        #endregion

        #region Static method: GetCurrentContentTitle()

        [Test]
        public void GetCurrentContentTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Content.Titles.AdministrationAccountsDetails;
            String actual = ResourceProvider.GetCurrentContentTitle();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCurrentContentTitle_GetsTitleWithoutArea()
        {
            routeValues["controller"] = "Profile";
            routeValues["action"] = "Delete";
            routeValues["area"] = null;

            String expected = MvcTemplate.Resources.Content.Titles.ProfileDelete;
            String actual = ResourceProvider.GetCurrentContentTitle();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCurrentContentTitle_OnContentNotFoundReturnsNull()
        {
            routeValues["area"] = null;

            Assert.IsNull(ResourceProvider.GetCurrentContentTitle());
        }

        #endregion

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

        #region Static method: GetPrivilegeControllerTitle(String controller)

        [Test]
        public void GetPrivilegeControllerTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Controller.Titles.Roles;
            String actual = ResourceProvider.GetPrivilegeControllerTitle("Roles");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPrivilegeControllerTitle_OnActionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPrivilegeControllerTitle("Test"));
        }

        #endregion

        #region Static method: GetPrivilegeActionTitle(String controller)

        [Test]
        public void GetPrivilegeActionTitle_GetsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Action.Titles.Index;
            String actual = ResourceProvider.GetPrivilegeActionTitle("Index");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPrivilegeActionTitle_OnActionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPrivilegeActionTitle("Test"));
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

        #region Static method: GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)

        [Test]
        public void GetPropertyTitle_OnNotMemberExpressionThrows()
        {
            Exception expected = Assert.Throws<InvalidOperationException>(() => ResourceProvider.GetPropertyTitle<TestView, String>(view => view.ToString()));
            Assert.AreEqual(expected.Message, "Expression must be a member expression");
        }

        [Test]
        public void GetPropertyTitle_GetsTitleFromExpression()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountView, String>(profile => profile.Username);
            String expected = MvcTemplate.Resources.Views.AccountView.Titles.Username;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Ignore]
        public void GetPropertyTitle_GetsTitleFromExpressionRelation()
        {
            String expected = "Expected";
            String actual = "Actual";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPropertyTitle_OnPropertyFromExpressionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPropertyTitle<AccountView, String>(profile => profile.Id));
        }

        [Test]
        public void GetPropertyTitle_OnTypeFromExpressionNotFoundReturnsNull()
        {
            Assert.IsNull(ResourceProvider.GetPropertyTitle<TestView, String>(model => model.Text));
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
