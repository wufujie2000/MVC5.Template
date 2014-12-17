using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class WidgetBoxExtensionsTests
    {
        private UrlHelper urlHelper;
        private HtmlHelper html;

        private String controller;
        private String accountId;
        private String area;

        [SetUp]
        public void SetUp()
        {
            html = HtmlHelperFactory.CreateHtmlHelper();
            urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            controller = html.ViewContext.RouteData.Values["controller"] as String;
            accountId = html.ViewContext.HttpContext.User.Identity.Name;
            area = html.ViewContext.RouteData.Values["area"] as String;
        }

        [TearDown]
        public void TearDown()
        {
            Authorization.Provider = null;
        }

        #region Extension method: WidgetButton(this HtmlHelper html, String action, String iconClass)

        [Test]
        public void WidgetButton_OnNotAuthorizedReturnsEmpty()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(accountId, area, controller, "Delete").Returns(false);

            String actual = html.WidgetButton("Delete", "icon").ToString();
            String expected = "";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WidgetButton_OnNullAuthorizationProviderFormsWidgetButton()
        {
            Authorization.Provider = null;

            String actual = html.WidgetButton("Create", "icon").ToString();
            String expected = String.Format(
                "<a class=\"btn\" href=\"{0}\">" +
                    "<i class=\"icon\"></i>" +
                    "<span class=\"text\">{1}</span>" +
                "</a>",
                urlHelper.Action("Create"),
                ResourceProvider.GetActionTitle("Create"));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WidgetButton_FormsAuthorizedWidgetButton()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(accountId, area, controller, "Delete").Returns(true);

            String actual = html.WidgetButton("Delete", "icon").ToString();
            String expected = String.Format(
                "<a class=\"btn\" href=\"{0}\">" +
                    "<i class=\"icon\"></i>" +
                    "<span class=\"text\">{1}</span>" +
                "</a>",
                urlHelper.Action("Delete"),
                ResourceProvider.GetActionTitle("Delete"));

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}