using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class WidgetBoxExtensionsTests
    {
        private HtmlHelper html;

        [SetUp]
        public void SetUp()
        {
            html = HtmlHelperFactory.CreateHtmlHelper();
        }

        [TearDown]
        public void TearDown()
        {
            Authorization.Provider = null;
        }

        #region Extension method: WidgetButtons(this HtmlHelper html, params LinkAction[] actions)

        [Test]
        public void WidgetButtons_FormsWidgetBoxButtons()
        {
            LinkAction[] actions = Enum.GetValues(typeof(LinkAction)).Cast<LinkAction>().ToArray();
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            Authorization.Provider = null;

            String actual = html.WidgetButtons(actions).ToString();
            String expected = String.Format(
                "<a class=\"btn\" href=\"{0}\">" +
                    "<i class=\"fa fa-file-o\"></i>" +
                    "<span class=\"text\">{1}</span>" +
                "</a>" + 
                "<a class=\"btn\" href=\"{2}\">" +
                    "<i class=\"fa fa-info\"></i>" +
                    "<span class=\"text\">{3}</span>" +
                "</a>" + 
                "<a class=\"btn\" href=\"{4}\">" +
                    "<i class=\"fa fa-pencil\"></i>" +
                    "<span class=\"text\">{5}</span>" +
                "</a>" + 
                "<a class=\"btn\" href=\"{6}\">" +
                    "<i class=\"fa fa-times\"></i>" +
                    "<span class=\"text\">{7}</span>" +
                "</a>" + 
                "<a class=\"btn\" href=\"{8}\">" +
                    "<i class=\"fa fa-files-o\"></i>" +
                    "<span class=\"text\">{9}</span>" +
                "</a>",
                urlHelper.Action("Create"),
                ResourceProvider.GetActionTitle("Create"),
                urlHelper.Action("Details"),
                ResourceProvider.GetActionTitle("Details"),
                urlHelper.Action("Edit"),
                ResourceProvider.GetActionTitle("Edit"),
                urlHelper.Action("Delete"),
                ResourceProvider.GetActionTitle("Delete"),
                urlHelper.Action("Copy"),
                ResourceProvider.GetActionTitle("Copy"));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TableWidgetBox_FormsWidgetBoxWithAuthorizedButtons()
        {
            String controller = html.ViewContext.RouteData.Values["controller"] as String;
            String accountId = html.ViewContext.HttpContext.User.Identity.Name;
            String area = html.ViewContext.RouteData.Values["area"] as String;

            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(accountId, area, controller, "Delete").Returns(true);
            Authorization.Provider.IsAuthorizedFor(accountId, area, controller, "Details").Returns(true);

            LinkAction[] actions = Enum.GetValues(typeof(LinkAction)).Cast<LinkAction>().ToArray();
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            String actual = html.WidgetButtons(actions).ToString();
            String expected = String.Format(
                "<a class=\"btn\" href=\"{0}\">" +
                    "<i class=\"fa fa-info\"></i>" +
                    "<span class=\"text\">{1}</span>" +
                "</a>" +
                "<a class=\"btn\" href=\"{2}\">" +
                    "<i class=\"fa fa-times\"></i>" +
                    "<span class=\"text\">{3}</span>" +
                "</a>",
                urlHelper.Action("Details"),
                ResourceProvider.GetActionTitle("Details"),
                urlHelper.Action("Delete"),
                ResourceProvider.GetActionTitle("Delete"));

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}