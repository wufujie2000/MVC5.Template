using NUnit.Framework;
using System;
using System.Web;
using System.Web.Routing;
using Template.Objects;
using Template.Resources;
using Template.Tests.Helpers;

namespace Template.Tests.Resources.Tests
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
            routeValues["area"] = "Administration";
            routeValues["controller"] = "Roles";

            Assert.AreEqual(Template.Resources.Form.Titles.AdministrationRoles, ResourceProvider.GetCurrentFormTitle());
        }

        [Test]
        public void GetCurrentFormTitle_GetsTitleWithoutArea()
        {
            routeValues["area"] = null;
            routeValues["controller"] = "Profile";
            
            Assert.AreEqual(Template.Resources.Form.Titles.Profile, ResourceProvider.GetCurrentFormTitle());
        }

        [Test]
        public void GetCurrentFormTitle_OnFormNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetCurrentFormTitle());
        }

        #endregion

        #region Static method: GetCurrentTableTitle()

        [Test]
        public void GetCurrentTableTitle_GetsTitle()
        {
            routeValues["area"] = "Administration";
            routeValues["controller"] = "Roles";
            routeValues["action"] = "Index";

            Assert.AreEqual(Template.Resources.Table.Titles.AdministrationRolesIndex, ResourceProvider.GetCurrentTableTitle());
        }

        [Test]
        [Ignore]
        public void GetCurrentTableTitle_GetsTitleWithoutArea()
        {
            Assert.Inconclusive("No table without area");
        }

        [Test]
        public void GetCurrentTableTitle_OnTableNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetCurrentTableTitle());
        }

        #endregion

        #region Static method: GetCurrentContentTitle()

        [Test]
        public void GetCurrentContentTitle_GetsTitle()
        {
            routeValues["area"] = "Administration";
            routeValues["controller"] = "Roles";
            routeValues["action"] = "Edit";

            Assert.AreEqual(Template.Resources.Content.Titles.AdministrationRolesEdit, ResourceProvider.GetCurrentContentTitle());
        }

        [Test]
        public void GetCurrentContentTitle_GetsTitleWithoutArea()
        {
            routeValues["area"] = null;
            routeValues["controller"] = "Profile";
            routeValues["action"] = "Delete";

            Assert.AreEqual(Template.Resources.Content.Titles.ProfileDelete, ResourceProvider.GetCurrentContentTitle());
        }

        [Test]
        public void GetCurrentContentTitle_OnContentNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetCurrentContentTitle());
        }

        #endregion

        #region Static method: GetActionTitle(String action)

        [Test]
        public void GetActionTitle_GetsTitle()
        {
            Assert.AreEqual(Template.Resources.Action.Titles.Create, ResourceProvider.GetActionTitle("Create"));
        }

        [Test]
        public void GetActionTitle_OnActionNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetActionTitle("Test"));
        }

        #endregion

        #region Static method: GetDatalistTitle<TModel>() where TModel : class

        [Test]
        public void GetDatalistTitle_GetsTitle()
        {
            Assert.AreEqual(Template.Resources.Datalist.Titles.Role, ResourceProvider.GetDatalistTitle<Role>());
        }

        [Test]
        public void GetDatalistTitle_OnTypeNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetDatalistTitle<Object>());
        }

        #endregion

        #region Static method: GetPrivilegeAreaTitle(String area)

        [Test]
        public void GetPrivilegeAreaTitle_GetsTitle()
        {
            Assert.AreEqual(Template.Resources.Privilege.Area.Titles.Administration, ResourceProvider.GetPrivilegeAreaTitle("Administration"));
        }

        [Test]
        public void GetPrivilegeAreaTitle_OnActionNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetPrivilegeAreaTitle("Test"));
        }

        #endregion

        #region Static method: GetPrivilegeControllerTitle(String controller)

        [Test]
        public void GetPrivilegeControllerTitle_GetsTitle()
        {
            Assert.AreEqual(Template.Resources.Privilege.Controller.Titles.Roles, ResourceProvider.GetPrivilegeControllerTitle("Roles"));
        }

        [Test]
        public void GetPrivilegeControllerTitle_OnActionNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetPrivilegeControllerTitle("Test"));
        }

        #endregion

        #region Static method: GetPrivilegeActionTitle(String controller)

        [Test]
        public void GetPrivilegeActionTitle_GetsTitle()
        {
            Assert.AreEqual(Template.Resources.Privilege.Action.Titles.Index, ResourceProvider.GetPrivilegeActionTitle("Index"));
        }

        [Test]
        public void GetPrivilegeActionTitle_OnActionNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetPrivilegeActionTitle("Test"));
        }

        #endregion

        #region Static method: GetMenuTitle(String area, String controller, String action)

        [Test]
        public void GetMenuTitle_GetsTitle()
        {
            Assert.AreEqual(
                Template.Resources.Menu.Titles.AdministrationRolesIndex,
                ResourceProvider.GetMenuTitle("Administration", "Roles", "Index"));
        }

        [Test]
        public void GetMenuTitle_GetsTitleWithoutControllerAndAction()
        {
            Assert.AreEqual(
                Template.Resources.Menu.Titles.Administration,
                ResourceProvider.GetMenuTitle("Administration", null, null));
        }

        [Test]
        public void GetMenuTitle_OnMenuNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetMenuTitle("Test", "Test", "Test"));
        }

        #endregion

        #region Static method: GetPropertyTitle<T, TKey>(Expression<Func<T, TKey>> property)

        [Test]
        public void GetPropertyTitle_GetsTitleFromExpression()
        {
            Assert.AreEqual(
                Template.Resources.Views.ProfileView.Titles.Username,
                ResourceProvider.GetPropertyTitle<ProfileView, String>(profile => profile.Username));
        }

        [Test]
        [Ignore]
        public void GetPropertyTitle_GetsTitleFromFromExpressionRelation()
        {
            /*
            Assert.AreEqual( 
                Template.Resources.Views.RoleView.Titles.Name,
                ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.role.FirstName));*/
        }

        [Test]
        public void GetPropertyTitle_OnPropertyFromExpressionNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetPropertyTitle<ProfileView, String>(profile => profile.Id));
        }

        [Test]
        public void GetPropertyTitle_OnTypeFromExpressionNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetPropertyTitle<NoResourcesModel, String>(model => model.Title));
        }

        #endregion

        #region Static method: GetPropertyTitle(Type viewType, String propertyName)

        [Test]
        public void GetPropertyTitle_GetsTitle()
        {
            Assert.AreEqual(
                Template.Resources.Views.ProfileView.Titles.Username,
                ResourceProvider.GetPropertyTitle(typeof(ProfileView), "Username"));
        }

        [Test]
        public void GetPropertyTitle_GetsTitleFromRelation()
        {
            Assert.AreEqual(
                Template.Resources.Views.RoleView.Titles.Name,
                ResourceProvider.GetPropertyTitle(typeof(RoleView), "RoleName"));
        }

        [Test]
        public void GetPropertyTitle_OnPropertyNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetPropertyTitle(typeof(ProfileView), "Id"));
        }

        [Test]
        public void GetPropertyTitle_OnTypeNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetPropertyTitle(typeof(NoResourcesModel), "Title"));
        }

        #endregion
    }
}
