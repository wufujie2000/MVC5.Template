using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class HeaderExtensionsTests
    {
        private UrlHelper urlHelper;
        private HtmlHelper html;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            html = HtmlHelperFactory.CreateHtmlHelper();
            urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            LocalizationManager.Provider = LanguageProviderFactory.CreateProvider();
        }

        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            LocalizationManager.Provider = null;
        }

        #region Extension method: ProfileLink(this HtmlHelper html)

        [Test]
        public void ProfileLink_FormsProfileLink()
        {
            String actual = html.ProfileLink().ToString();
            String expected = String.Format(
                "<a href=\"{0}\"><i class=\"fa fa-user\"></i><span>{1}</span></a>",
                urlHelper.Action("Edit", "Profile", new { area = String.Empty }),
                ResourceProvider.GetActionTitle("Profile"));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: LanguageLink(this HtmlHelper html)

        [Test]
        public void LanguageLink_OnSingleLanguageReturnsEmpty()
        {
            LocalizationManager.Provider = LanguageProviderFactory.CreateProvider();
            LocalizationManager.Provider.Languages.Returns(new[] { new Language() });

            String actual = html.LanguageLink().ToString();
            String expected = String.Empty;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LanguageLink_FormsLanguageLink()
        {
            RouteValueDictionary routeValues = html.ViewContext.RequestContext.RouteData.Values;
            String action = routeValues["action"].ToString();

            String actual = html.LanguageLink().ToString();
            String expected = String.Format(
                "<a class=\"dropdown-toggle\" data-toggle=\"dropdown\">" +
                    "<i class=\"fa fa-flag\"></i>{0}<span class=\"caret\"></span>" +
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
                urlHelper.Action(action, new { language = "en", p = "1" }),
                urlHelper.Action(action, new { language = "lt", p = "1" }),
                urlHelper.Content("~/Images/Flags/en.gif"),
                urlHelper.Content("~/Images/Flags/lt.gif"));

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
                     "<span>{1}</span>" +
                "</a>",
                urlHelper.Action("Logout", "Auth", new { area = String.Empty }),
                ResourceProvider.GetActionTitle("Logout"));

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
