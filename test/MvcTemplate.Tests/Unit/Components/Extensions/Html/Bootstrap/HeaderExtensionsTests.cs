using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class HeaderExtensionsTests
    {
        private HtmlHelper html;
        private UrlHelper url;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            html = HtmlHelperFactory.CreateHtmlHelper();
            url = new UrlHelper(html.ViewContext.RequestContext);
        }

        [SetUp]
        public void SetUp()
        {
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            GlobalizationManager.Provider = null;
        }

        #region Extension method: ProfileLink(this HtmlHelper html)

        [Test]
        public void ProfileLink_FormsProfileLink()
        {
            String actual = html.ProfileLink().ToString();
            String expected = String.Format(
                "<a href=\"{0}\">" +
                    "<i class=\"fa fa-user\"></i>" +
                    "<span class=\"text\">{1}</span>" +
                "</a>",
                url.Action("Edit", "Profile", new { area = "" }),
                ResourceProvider.GetActionTitle("Profile"));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: LanguageLink(this HtmlHelper html)

        [Test]
        public void LanguageLink_OnSingleLanguageReturnsEmpty()
        {
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
            GlobalizationManager.Provider.Languages.Returns(new[] { new Language() });

            String actual = html.LanguageLink().ToString();
            String expected = "";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LanguageLink_FormsLanguageLink()
        {
            html.ViewContext.HttpContext.Request.QueryString.Returns(HttpUtility.ParseQueryString(""));
            RouteValueDictionary routeValues = html.ViewContext.RouteData.Values;
            String action = routeValues["action"].ToString();

            String actual = html.LanguageLink().ToString();
            String expected = String.Format(
                "<a class=\"dropdown-toggle\" data-toggle=\"dropdown\">" +
                    "<i class=\"fa fa-globe\"></i>" +
                    "<span class=\"text\">{0}</span>" +
                    "<span class=\"caret\"></span>" +
                "</a>" +
                "<ul class=\"dropdown-menu\" role=\"menu\">" +
                    "<li>" +
                        "<a href=\"{1}\"><img src=\"{3}\" alt=\"\" />English</a>" +
                    "</li>" +
                    "<li>" +
                        "<a href=\"{2}\"><img src=\"{4}\" alt=\"\" />Lietuvių</a>" +
                    "</li>" +
                "</ul>",
                ResourceProvider.GetActionTitle("Language"),
                url.Action(action, new { area = routeValues["area"], language = "en" }),
                url.Action(action, new { area = routeValues["area"], language = "lt" }),
                url.Content("~/Images/Flags/en.gif"),
                url.Content("~/Images/Flags/lt.gif"));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: LogoutLink(this HtmlHelper html)

        [Test]
        public void LogoutLink_FormsLogoutLink()
        {
            String actual = html.LogoutLink().ToString();
            String expected = String.Format(
                "<a href=\"{0}\">" +
                    "<i class=\"fa fa-share\"></i>" +
                     "<span class=\"text\">{1}</span>" +
                "</a>",
                url.Action("Logout", "Auth", new { area = "" }),
                ResourceProvider.GetActionTitle("Logout"));

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
