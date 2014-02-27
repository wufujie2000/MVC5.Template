using NUnit.Framework;
using System;
using System.Web;
using System.Web.Routing;
using Template.Objects;
using Template.Objects.Resources;
using Template.Resources;
using Tests.Helpers;

namespace Template.Tests.Resources.Tests
{
    [TestFixture]
    public class ResourceProviderTests
    {
        private RouteValueDictionary routeValues; 

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContextStub().Context;
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
            routeValues["controller"] = "Users";

            Assert.AreEqual(Template.Resources.Form.Titles.AdministrationUsers, ResourceProvider.GetCurrentFormTitle());
        }

        [Test]
        public void GetCurrentFormTitle_GetsTitleWithoutArea()
        {
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
            routeValues["controller"] = "Users";
            routeValues["action"] = "Index";

            Assert.AreEqual(Template.Resources.Table.Titles.AdministrationUsersIndex, ResourceProvider.GetCurrentTableTitle());
        }

        [Test]
        [Ignore]
        public void GetCurrentTableTitle_GetsTitleWithoutArea()
        {
            throw new NotImplementedException("No table without area");
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
            routeValues["controller"] = "Users";
            routeValues["action"] = "Edit";

            Assert.AreEqual(Template.Resources.Content.Titles.AdministrationUsersEdit, ResourceProvider.GetCurrentContentTitle());
        }

        [Test]
        public void GetCurrentContentTitle_GetsTitleWithoutArea()
        {
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

        #region Static method: GetDatalistTitle(String type)

        [Test]
        public void GetDatalistTitle_GetsTitle()
        {
            Assert.AreEqual(Template.Resources.Datalist.Titles.Roles, ResourceProvider.GetDatalistTitle("Roles"));
        }

        [Test]
        public void GetDatalistTitle_OnTypeNotFoundReturnsEmpty()
        {
            Assert.AreEqual(String.Empty, ResourceProvider.GetDatalistTitle("Test"));
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
        public void GetPropertyTitle_GetsTitleFromFromExpressionRelation()
        {
            Assert.AreEqual(
                Template.Resources.Views.UserView.Titles.FirstName,
                ResourceProvider.GetPropertyTitle<ProfileView, String>(profile => profile.UserFirstName));
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
                Template.Resources.Views.UserView.Titles.FirstName,
                ResourceProvider.GetPropertyTitle(typeof(ProfileView), "UserFirstName"));
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
