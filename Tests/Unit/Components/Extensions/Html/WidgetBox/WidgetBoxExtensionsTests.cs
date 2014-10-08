using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

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
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
        }

        [TearDown]
        public void TearDown()
        {
            Authorization.Provider = null;
            HttpContext.Current = null;
        }

        #region Extension method: TableWidgetBox(this HtmlHelper html, params LinkAction[] actions)

        [Test]
        public void TableWidgetBox_FormsTableWidgetBox()
        {
            StringBuilder expected = new StringBuilder();
            StringBuilder actual = new StringBuilder();

            new WidgetBox(new StringWriter(expected), "fa fa-th", ResourceProvider.GetCurrentTableTitle(), String.Empty).Dispose();
            html.ViewContext.Writer = new StringWriter(actual);
            html.TableWidgetBox().Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void TableWidgetBox_FormsTableWidgetBoxWithButtons()
        {
            StringBuilder expected = new StringBuilder();
            StringBuilder actual = new StringBuilder();
            Authorization.Provider = null;

            String buttons = FormTitleButtons(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete);
            new WidgetBox(new StringWriter(expected), "fa fa-th", ResourceProvider.GetCurrentTableTitle(), buttons).Dispose();

            html.ViewContext.Writer = new StringWriter(actual);
            html.TableWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void TableWidgetBox_FormsTableWidgetBoxWithAuthorizedButtons()
        {
            Authorization
                .Provider
                .IsAuthorizedFor(
                       Arg.Any<String>(),
                       Arg.Any<String>(),
                       Arg.Any<String>(),
                       Arg.Is<String>(value => new[] { "Details", "Delete" }.Contains(value)))
                .Returns(true);

            StringBuilder expected = new StringBuilder();
            StringBuilder actual = new StringBuilder();

            String buttons = FormTitleButtons(LinkAction.Details, LinkAction.Delete);
            new WidgetBox(new StringWriter(expected), "fa fa-th", ResourceProvider.GetCurrentTableTitle(), buttons).Dispose();

            html.ViewContext.Writer = new StringWriter(actual);
            html.TableWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Extension method: FormWidgetBox(this HtmlHelper html, params LinkAction[] actions)

        [Test]
        public void FormWidgetBox_FormsWidgetBox()
        {
            StringBuilder expected = new StringBuilder();
            new WidgetBox(new StringWriter(expected), "fa fa-th-list", ResourceProvider.GetCurrentFormTitle(), String.Empty).Dispose();

            StringBuilder actual = new StringBuilder();
            html.ViewContext.Writer = new StringWriter(actual);
            html.FormWidgetBox().Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void FormWidgetBox_FormsWidgetBoxWithButtons()
        {
            StringBuilder expected = new StringBuilder();
            StringBuilder actual = new StringBuilder();
            Authorization.Provider = null;

            String buttons = FormTitleButtons(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete);
            new WidgetBox(new StringWriter(expected), "fa fa-th-list", ResourceProvider.GetCurrentFormTitle(), buttons).Dispose();

            html.ViewContext.Writer = new StringWriter(actual);
            html.FormWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void FormWidgetBox_FormsWidgetBoxWithAuthorizedButtons()
        {
            Authorization
                .Provider
                .IsAuthorizedFor(
                    Arg.Any<String>(),
                    Arg.Any<String>(),
                    Arg.Any<String>(),
                    Arg.Is<String>(value => new[] { "Create", "Edit" }.Contains(value)))
                .Returns(true);

            StringBuilder expected = new StringBuilder();
            StringBuilder actual = new StringBuilder();

            String buttons = FormTitleButtons(LinkAction.Create, LinkAction.Edit);
            new WidgetBox(new StringWriter(expected), "fa fa-th-list", ResourceProvider.GetCurrentFormTitle(), buttons).Dispose();

            html.ViewContext.Writer = new StringWriter(actual);
            html.FormWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Edit).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Test helpers

        private String FormTitleButtons(params LinkAction[] actions)
        {
            String buttons = String.Empty;
            foreach (LinkAction action in actions)
            {
                TagBuilder icon = new TagBuilder("i");
                switch (action)
                {
                    case LinkAction.Create:
                        icon.AddCssClass("fa fa-file-o");
                        break;
                    case LinkAction.Details:
                        icon.AddCssClass("fa fa-info");
                        break;
                    case LinkAction.Edit:
                        icon.AddCssClass("fa fa-pencil");
                        break;
                    case LinkAction.Delete:
                        icon.AddCssClass("fa fa-times");
                        break;
                }

                String button = String.Format(
                    html.ActionLink(
                            "{0}{1}",
                            action.ToString(),
                            new { id = html.ViewContext.RouteData.Values["id"] },
                            new { @class = "btn" })
                        .ToString(),
                    icon,
                    ResourceProvider.GetActionTitle(action.ToString()));

                buttons += button;
            }

            return buttons;
        }

        #endregion
    }
}
