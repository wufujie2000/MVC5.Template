using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Xml.Linq;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class MvcSiteMapProviderTests
    {
        private IEnumerable<MvcSiteMapNode> siteMapTree;
        private RouteValueDictionary routeValues;
        private MvcSiteMapProvider provider;
        private MvcSiteMapParser parser;
        private String siteMapPath;
        private XElement siteMap;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            siteMapPath = "MvcSiteMapProviderTests.sitemap";
            parser = new MvcSiteMapParser();
            siteMap = CreateSiteMap();
            siteMap.Save(siteMapPath);

            siteMapTree = parser.GetNodeTree(siteMap);
        }

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            provider = new MvcSiteMapProvider(siteMapPath, parser);
        }

        [TearDown]
        public void TearDown()
        {
            Authorization.Provider = null;
            HttpContext.Current = null;
        }

        #region Method: GetAuthorizedMenus()

        [Test]
        public void GetAuthorizedMenus_OnNullAuthorizationProviderReturnsAllMenus()
        {
            Authorization.Provider = null;

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus().ToArray();

            Assert.AreEqual(1, actual.Length);

            Assert.AreEqual(null, actual[0].Action);
            Assert.AreEqual(null, actual[0].Controller);
            Assert.AreEqual("Administration", actual[0].Area);
            Assert.AreEqual("fa fa-gears", actual[0].IconClass);

            actual = actual[0].Children.ToArray();

            Assert.AreEqual(2, actual.Length);

            Assert.AreEqual(0, actual[0].Children.Count());

            Assert.AreEqual("Index", actual[0].Action);
            Assert.AreEqual("Accounts", actual[0].Controller);
            Assert.AreEqual("Administration", actual[0].Area);
            Assert.AreEqual("fa fa-user", actual[0].IconClass);

            Assert.AreEqual(null, actual[1].Action);
            Assert.AreEqual("Roles", actual[1].Controller);
            Assert.AreEqual("Administration", actual[1].Area);
            Assert.AreEqual("fa fa-users", actual[1].IconClass);

            actual = actual[1].Children.ToArray();

            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(0, actual[0].Children.Count());

            Assert.AreEqual("Create", actual[0].Action);
            Assert.AreEqual("Roles", actual[0].Controller);
            Assert.AreEqual("Administration", actual[0].Area);
            Assert.AreEqual("fa fa-file-o", actual[0].IconClass);
        }

        [Test]
        public void GetAuthorizedMenus_ReturnsOnlyAuthorizedMenus()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(HttpContext.Current.User.Identity.Name, "Administration", "Accounts", "Index").Returns(true);

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus().ToArray();

            Assert.AreEqual(1, actual.Length);

            Assert.AreEqual(null, actual[0].Action);
            Assert.AreEqual(null, actual[0].Controller);
            Assert.AreEqual("Administration", actual[0].Area);
            Assert.AreEqual("fa fa-gears", actual[0].IconClass);

            actual = actual[0].Children.ToArray();

            Assert.AreEqual(1, actual.Length);

            Assert.AreEqual(0, actual[0].Children.Count());

            Assert.AreEqual("Index", actual[0].Action);
            Assert.AreEqual("Accounts", actual[0].Controller);
            Assert.AreEqual("Administration", actual[0].Area);
            Assert.AreEqual("fa fa-user", actual[0].IconClass);
        }

        #endregion

        [Test]
        public void GetAuthorizedMenus_SetsActiveMenu()
        {
            Authorization.Provider = null;
            routeValues["action"] = "Create";
            routeValues["controller"] = "Roles";
            routeValues["area"] = "Administration";

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus().ToArray();

            Assert.AreEqual(1, actual.Length);
            Assert.IsFalse(actual[0].IsActive);

            actual = actual[0].Children.ToArray();

            Assert.AreEqual(2, actual.Length);
            Assert.IsFalse(actual[0].IsActive);
            Assert.IsFalse(actual[1].IsActive);
            Assert.AreEqual(0, actual[0].Children.Count());

            actual = actual[1].Children.ToArray();

            Assert.IsTrue(actual[0].IsActive);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(0, actual[0].Children.Count());
        }

        [Test]
        public void GetAuthorizedMenus_SetsActiveMenuIfNonMenuChildrenNodeIsActive()
        {
            Authorization.Provider = null;
            routeValues["action"] = "Edit";
            routeValues["controller"] = "Accounts";
            routeValues["area"] = "Administration";

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus().ToArray();

            Assert.AreEqual(1, actual.Length);
            Assert.IsFalse(actual[0].IsActive);

            actual = actual[0].Children.ToArray();

            Assert.AreEqual(2, actual.Length);
            Assert.IsTrue(actual[0].IsActive);
            Assert.IsFalse(actual[1].IsActive);
            Assert.AreEqual(0, actual[0].Children.Count());

            actual = actual[1].Children.ToArray();

            Assert.AreEqual(1, actual.Length);
            Assert.IsFalse(actual[0].IsActive);
            Assert.AreEqual(0, actual[0].Children.Count());
        }

        [Test]
        public void GetAuthorizedMenus_SetsHasActiveChildrenOnEveryActiveMenuParents()
        {
            Authorization.Provider = null;
            routeValues["action"] = "Create";
            routeValues["controller"] = "Roles";
            routeValues["area"] = "Administration";

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus().ToArray();

            Assert.AreEqual(1, actual.Length);
            Assert.IsTrue(actual[0].HasActiveChildren);

            actual = actual[0].Children.ToArray();

            Assert.AreEqual(2, actual.Length);
            Assert.IsTrue(actual[1].HasActiveChildren);
            Assert.IsFalse(actual[0].HasActiveChildren);
            Assert.AreEqual(0, actual[0].Children.Count());

            actual = actual[1].Children.ToArray();

            Assert.AreEqual(1, actual.Length);
            Assert.IsFalse(actual[0].HasActiveChildren);
            Assert.AreEqual(0, actual[0].Children.Count());
        }

        [Test]
        public void GetAuthorizedMenus_RemovesEmptyMenus()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>()).Returns(true);
            Authorization.Provider.IsAuthorizedFor(HttpContext.Current.User.Identity.Name, "Administration", "Roles", "Create").Returns(false);

            MvcSiteMapNode[] actual = provider.GetAuthorizedMenus().ToArray();

            Assert.AreEqual(1, actual.Length);

            Assert.AreEqual(null, actual[0].Action);
            Assert.AreEqual(null, actual[0].Controller);
            Assert.AreEqual("Administration", actual[0].Area);
            Assert.AreEqual("fa fa-gears", actual[0].IconClass);

            actual = actual[0].Children.ToArray();

            Assert.AreEqual(1, actual.Length);

            Assert.AreEqual(0, actual[0].Children.Count());

            Assert.AreEqual("Index", actual[0].Action);
            Assert.AreEqual("Accounts", actual[0].Controller);
            Assert.AreEqual("Administration", actual[0].Area);
            Assert.AreEqual("fa fa-user", actual[0].IconClass);
        }

        #region Method: GetBreadcrumb()

        [Test]
        public void GetBreadcrumb_FormsBreadcrumbByIgnoringCase()
        {
            routeValues["controller"] = "profile";
            routeValues["action"] = "edit";
            routeValues["area"] = null;

            MvcSiteMapNode[] actual = provider.GetBreadcrumb().ToArray();

            Assert.AreEqual(3, actual.Length);

            Assert.AreEqual("fa fa-home", actual[0].IconClass);
            Assert.AreEqual("Home", actual[0].Controller);
            Assert.AreEqual("Index", actual[0].Action);
            Assert.AreEqual(null, actual[0].Area);

            Assert.AreEqual("fa fa-user", actual[1].IconClass);
            Assert.AreEqual("Profile", actual[1].Controller);
            Assert.AreEqual(null, actual[1].Action);
            Assert.AreEqual(null, actual[1].Area);

            Assert.AreEqual("fa fa-pencil", actual[2].IconClass);
            Assert.AreEqual("Profile", actual[2].Controller);
            Assert.AreEqual("Edit", actual[2].Action);
            Assert.AreEqual(null, actual[2].Area);
        }

        [Test]
        public void GetBreadcrumb_OnNotFoundCurrentActionReturnsEmpty()
        {
            routeValues["controller"] = "profile";
            routeValues["action"] = "edit";
            routeValues["area"] = "area";

            CollectionAssert.IsEmpty(provider.GetBreadcrumb());
        }

        #endregion

        #region Test helpers

        private XElement CreateSiteMap()
        {
            return XElement.Parse(
            @"<siteMap>
                <siteMapNode icon=""fa fa-home"" controller=""Home"" action=""Index"">
                    <siteMapNode icon=""fa fa-user"" controller=""Profile"">
                        <siteMapNode icon=""fa fa-pencil"" controller=""Profile"" action=""Edit"" />
                    </siteMapNode>
                    <siteMapNode menu=""true"" icon=""fa fa-gears"" area=""Administration"">
                        <siteMapNode menu=""true"" icon=""fa fa-user"" area=""Administration"" controller=""Accounts"" action=""Index"">
                            <siteMapNode icon=""fa fa-info"" area=""Administration"" controller=""Accounts"" action=""Details"">
                                <siteMapNode icon=""fa fa-pencil"" area=""Administration"" controller=""Accounts"" action=""Edit"" />
                            </siteMapNode>
                        </siteMapNode>
                        <siteMapNode menu=""true"" icon=""fa fa-users"" area=""Administration"" controller=""Roles"">
                            <siteMapNode menu=""true"" icon=""fa fa-file-o"" area=""Administration"" controller=""Roles"" action=""Create"" />
                            <siteMapNode icon=""fa fa-pencil"" area=""Administration"" controller=""Roles"" action=""Edit"" />
                        </siteMapNode>
                    </siteMapNode>
                </siteMapNode>
            </siteMap>");
        }

        #endregion
    }
}
