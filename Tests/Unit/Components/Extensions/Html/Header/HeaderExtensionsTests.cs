using NUnit.Framework;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Components.Extensions.Html;
using Template.Resources;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class HeaderExtensionsTests
    {
        private HtmlHelper html;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            html = new HtmlMock().Html;
        }

        #region Extension method: ProfileLink(this HtmlHelper html)

        [Test]
        public void ProfileLink_FormsProfileLink()
        {
            String expected = String.Format("<a href=\"{0}\"><i class=\"fa fa-user\"></i><span>{1}</span></a>",
                new UrlHelper(html.ViewContext.RequestContext).Action("Edit", new { controller = "Profile", area = String.Empty }),
                ResourceProvider.GetActionTitle("Profile"));
            String actual = html.ProfileLink().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: LanguageLink(this HtmlHelper html)

        [Test]
        public void LanguageLink_FormsLanguageLink()
        {
            RouteValueDictionary routeValues = html.ViewContext.RequestContext.RouteData.Values;
            String controller = routeValues["controller"].ToString();
            String action = routeValues["action"].ToString();
            String area = routeValues["area"].ToString();

            String expected = String.Format("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\">"
                + "<i class=\"fa fa-flag\"></i> {0} <span class=\"caret\"></span></a>"
                + "<ul class=\"dropdown-menu\" role=\"menu\"><li>"
                + "<a href=\"{1}\"><img src='/Images/Flags/en-GB.gif' /> English</a></li><li>"
                + "<a href=\"{2}\"><img src='/Images/Flags/lt-LT.gif' /> Lietuvių</a></li></ul>",
                ResourceProvider.GetActionTitle("Language"),
                new UrlHelper(html.ViewContext.RequestContext).Action(action, new { language = "en-GB", controller = controller, area = area }),
                new UrlHelper(html.ViewContext.RequestContext).Action(action, new { language = "lt-LT", controller = controller, area = area }));
            
            Assert.AreEqual(expected, html.LanguageLink().ToString());
        }

        #endregion

        #region Extension method: LogoutLink(this HtmlHelper html)

        [Test]
        public void LogoutLink_FormsLogoutLink()
        {
            String expected = String.Format("<a href=\"{0}\"><i class=\"fa fa-share\"></i><span>{1}</span></a>",
                new UrlHelper(html.ViewContext.RequestContext).Action("Logout", new { controller = "Account", area = String.Empty }),
                ResourceProvider.GetActionTitle("Logout"));
            String actual = html.LogoutLink().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
