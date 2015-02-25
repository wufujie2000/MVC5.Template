using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using NSubstitute;
using System;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class WidgetBoxExtensionsTests : IDisposable
    {
        private UrlHelper urlHelper;
        private HtmlHelper html;

        private String controller;
        private String accountId;
        private String area;

        public WidgetBoxExtensionsTests()
        {
            html = HtmlHelperFactory.CreateHtmlHelper();
            urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            controller = html.ViewContext.RouteData.Values["controller"] as String;
            accountId = html.ViewContext.HttpContext.User.Identity.Name;
            area = html.ViewContext.RouteData.Values["area"] as String;
        }
        public void Dispose()
        {
            Authorization.Provider = null;
        }

        #region Extension method: WidgetButton(this HtmlHelper html, String action, String iconClass)

        [Fact]
        public void WidgetButton_OnNotAuthorizedReturnsEmpty()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(accountId, area, controller, "Delete").Returns(false);

            String actual = html.WidgetButton("Delete", "icon").ToString();
            String expected = "";

            Assert.Equal(expected, actual);
        }

        [Fact]
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

            Assert.Equal(expected, actual);
        }

        [Fact]
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

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}