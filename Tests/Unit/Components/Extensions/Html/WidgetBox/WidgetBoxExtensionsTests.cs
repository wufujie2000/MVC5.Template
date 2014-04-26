using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Template.Components.Extensions.Html;
using Template.Components.Security;
using Template.Resources;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class WidgetBoxExtensionsTests
    {
        private Mock<IRoleProvider> roleProviderMock;
        private HtmlHelper html;

        [SetUp]
        public void SetUp()
        {
            HtmlMock htmlMock = new HtmlMock();

            html = htmlMock.Html;
            roleProviderMock = new Mock<IRoleProvider>();
            HttpContext.Current = htmlMock.HttpMock.HttpContext;
            RoleProviderFactory.SetInstance(roleProviderMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            RoleProviderFactory.SetInstance(null);
            HttpContext.Current = null;
        }

        #region Extension method: TableWidgetBox(this HtmlHelper html, params LinkAction[] actions)

        [Test]
        public void TableWidgetBox_FormsTableWidgetBoxWithoutButtons()
        {
            StringBuilder expected = new StringBuilder();
            new WidgetBox(new StringWriter(expected), "fa fa-th", ResourceProvider.GetCurrentTableTitle(), String.Empty).Dispose();

            StringBuilder actual = new StringBuilder();
            html.ViewContext.Writer = new StringWriter(actual);
            html.TableWidgetBox().Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void TableWidgetBox_OnNullRoleProviderFormsTableWidgetBoxWithButtons()
        {
            RoleProviderFactory.SetInstance(null);

            StringBuilder expected = new StringBuilder();
            String buttons = FormTitleButtons(html, LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete);
            new WidgetBox(new StringWriter(expected), "fa fa-th", ResourceProvider.GetCurrentTableTitle(), buttons).Dispose();

            StringBuilder actual = new StringBuilder();
            html.ViewContext.Writer = new StringWriter(actual);
            html.TableWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void TableWidgetBox_FormsTableWidgetBoxWithButtons()
        {
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(
                   It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(true);

            StringBuilder expected = new StringBuilder();
            String buttons = FormTitleButtons(html, LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete);
            new WidgetBox(new StringWriter(expected), "fa fa-th", ResourceProvider.GetCurrentTableTitle(), buttons).Dispose();

            StringBuilder actual = new StringBuilder();
            html.ViewContext.Writer = new StringWriter(actual);
            html.TableWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void TableWidgetBox_FormsTableWidgetBoxWithAuthorizedButtons()
        {
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(
                   It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsIn<String>("Details", "Delete"))).Returns(true);

            StringBuilder expected = new StringBuilder();
            String buttons = FormTitleButtons(html, LinkAction.Details, LinkAction.Delete);
            new WidgetBox(new StringWriter(expected), "fa fa-th", ResourceProvider.GetCurrentTableTitle(), buttons).Dispose();

            StringBuilder actual = new StringBuilder();
            html.ViewContext.Writer = new StringWriter(actual);
            html.TableWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Extension method: FormWidgetBox(this HtmlHelper html, params LinkAction[] actions)

        [Test]
        public void FormWidgetBox_FormsFormWidgetBoxWithoutButtons()
        {
            StringBuilder expected = new StringBuilder();
            new WidgetBox(new StringWriter(expected), "fa fa-th-list", ResourceProvider.GetCurrentFormTitle(), String.Empty).Dispose();

            StringBuilder actual = new StringBuilder();
            html.ViewContext.Writer = new StringWriter(actual);
            html.FormWidgetBox().Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void FormWidgetBox_OnNullRoleProviderFormsFormWidgetBoxWithButtons()
        {
            RoleProviderFactory.SetInstance(null);

            StringBuilder expected = new StringBuilder();
            String buttons = FormTitleButtons(html, LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete);
            new WidgetBox(new StringWriter(expected), "fa fa-th-list", ResourceProvider.GetCurrentFormTitle(), buttons).Dispose();

            StringBuilder actual = new StringBuilder();
            html.ViewContext.Writer = new StringWriter(actual);
            html.FormWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void FormWidgetBox_FormsFormWidgetBoxWithAuthorizedButtons()
        {
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(
                It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(true);

            StringBuilder expected = new StringBuilder();
            String buttons = FormTitleButtons(html, LinkAction.Create, LinkAction.Details, LinkAction.Details);
            new WidgetBox(new StringWriter(expected), "fa fa-th-list", ResourceProvider.GetCurrentFormTitle(), buttons).Dispose();

            StringBuilder actual = new StringBuilder();
            html.ViewContext.Writer = new StringWriter(actual);
            html.FormWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Details).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public void FormWidgetBox_FormsFormWidgetBoxWithButtons()
        {
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(
                   It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsIn<String>("Details", "Delete"))).Returns(true);

            StringBuilder expected = new StringBuilder();
            String buttons = FormTitleButtons(html, LinkAction.Details, LinkAction.Delete);
            new WidgetBox(new StringWriter(expected), "fa fa-th-list", ResourceProvider.GetCurrentFormTitle(), buttons).Dispose();

            StringBuilder actual = new StringBuilder();
            html.ViewContext.Writer = new StringWriter(actual);
            html.FormWidgetBox(LinkAction.Create, LinkAction.Details, LinkAction.Edit, LinkAction.Delete).Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }


        #endregion

        #region Test helpers

        private String FormTitleButtons(HtmlHelper html, params LinkAction[] actions)
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
                        icon.AddCssClass("fa fa-edit");
                        break;
                    case LinkAction.Delete:
                        icon.AddCssClass("fa fa-times");
                        break;
                }

                TagBuilder span = new TagBuilder("span");
                span.InnerHtml = ResourceProvider.GetActionTitle(action.ToString());

                String button = String.Format(
                    html.ActionLink(
                            "{0}{1}",
                            action.ToString(),
                            new { id = html.ViewContext.RouteData.Values["id"] },
                            new { @class = "btn" })
                        .ToString(),
                    icon.ToString(),
                    span.ToString());

                buttons += button;
            }

            return buttons;
        }

        #endregion
    }
}
