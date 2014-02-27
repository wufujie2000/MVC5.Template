using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Template.Components.Extensions.Html;
using Template.Resources;
using Template.Tests.Helpers;

namespace Template.Tests.Tests.Components.Extensions.Html.Header
{
    [TestFixture]
    public class HeaderExtensionsTests
    {
        private HtmlHelperMock htmlMock;
        private HtmlHelper html;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            htmlMock = new HtmlHelperMock();
            html = htmlMock.Html;
        }

        #region Extension method: ProfileLink(this HtmlHelper html)

        [Test]
        public void ProfileLink_FormsProfileLink()
        {
            var icon = new TagBuilder("i");
            var span = new TagBuilder("span");
            icon.AddCssClass("fa fa-user");

            span.InnerHtml = ResourceProvider.GetActionTitle("Profile");
            
            var expected = String.Format(html.ActionLink("{0}{1}", "Edit", new { controller = "Profile", area = String.Empty }).ToString(), icon,  span);
            var actual = html.ProfileLink().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: LanguageLink(this HtmlHelper html)

        [Test]
        public void LanguageLink_FormsLanguageLink()
        {
            var action = new TagBuilder("a");
            var icon = new TagBuilder("i");
            var span = new TagBuilder("span");

            action.MergeAttribute("data-toggle", "dropdown");
            action.AddCssClass("dropdown-toggle");
            icon.AddCssClass("fa fa-flag");
            span.AddCssClass("caret");

            action.InnerHtml = String.Format("{0} {1} {2}", icon, ResourceProvider.GetActionTitle("Language"), span);

            var languageList = new TagBuilder("ul");
            languageList.MergeAttribute("role", "menu");
            languageList.AddCssClass("dropdown-menu");

            var languages = new Dictionary<String, String>()
            {
                { "en-GB", "English" },
                { "lt-LT", "Lietuvių" }
            };

            var queryString = new NameValueCollection();
            queryString.Add("Param1", "Value1");
            htmlMock.HttpRequestWrapperMock.Setup(mock => mock.QueryString).Returns(queryString);
            html.ViewContext.RequestContext.RouteData.Values["controller"] = "Test";
            html.ViewContext.RequestContext.RouteData.Values["action"] = "Test";
            html.ViewContext.RequestContext.RouteData.Values["Param1"] = "Value1";
            var currentLanguage = html.ViewContext.RequestContext.RouteData.Values["language"];
            foreach (var language in languages)
            {
                html.ViewContext.RequestContext.RouteData.Values["language"] = language.Key;
                TagBuilder languageItem = new TagBuilder("li");
                languageItem.InnerHtml = String.Format(
                    html
                        .ActionLink(
                            "{0} {1}",
                            html.ViewContext.RequestContext.RouteData.Values["action"].ToString(),
                            html.ViewContext.RequestContext.RouteData.Values)
                        .ToString(),
                    "<img src='/Images/Flags/" + language.Key + ".gif' />", language.Value);

                languageList.InnerHtml += languageItem.ToString();
            }

            html.ViewContext.RequestContext.RouteData.Values.Remove("Param1");
            html.ViewContext.RequestContext.RouteData.Values["language"] = currentLanguage;
            
            Assert.AreEqual(String.Format("{0}{1}", action, languageList), html.LanguageLink().ToString());
        }

        #endregion

        #region Extension method: LogoutLink(this HtmlHelper html)

        [Test]
        public void LogoutLink_FormsLogoutLink()
        {
            var icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-share");
            var span = new TagBuilder("span");

            span.InnerHtml = ResourceProvider.GetActionTitle("Logout");
            var expected = String.Format(html.ActionLink("{0}{1}", "Logout", new { controller = "Account", area = String.Empty }).ToString(), icon, span);
            var actual = html.LogoutLink().ToString();
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
