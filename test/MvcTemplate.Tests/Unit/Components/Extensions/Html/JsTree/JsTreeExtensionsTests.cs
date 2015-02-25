using MvcTemplate.Components.Extensions.Html;
using System;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class JsTreeViewExtensionsTests
    {
        private HtmlHelper<JsTreeView> html;

        public JsTreeViewExtensionsTests()
        {
            JsTreeView model = new JsTreeView();

            model.JsTree.SelectedIds.Add("1");
            model.JsTree.Nodes.Add(new JsTreeNode("Test"));
            model.JsTree.Nodes[0].Nodes.Add(new JsTreeNode("1", "Test1"));
            model.JsTree.Nodes[0].Nodes.Add(new JsTreeNode("2", "Test2"));

            html = HtmlHelperFactory.CreateHtmlHelper(model);
        }

        #region Extension method: JsTreeFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, JsTree>> expression)

        [Fact]
        public void JsTreeFor_FormsJsTreeFor()
        {
            String actual = html.JsTreeFor(model => model.JsTree).ToString();
            String expected =
                "<span class=\"js-tree-view-ids\">" +
                    "<input name=\"JsTree.SelectedIds\" type=\"hidden\" value=\"1\" />" +
                "</span>" +
                "<div class=\"js-tree-view\" for=\"JsTree.SelectedIds\">" +
                    "<ul>" +
                        "<li>Test" +
                            "<ul>" +
                                "<li id=\"1\">Test1</li>" +
                                "<li id=\"2\">Test2</li>" +
                            "</ul>" +
                        "</li>" +
                    "</ul>" +
                "</div>";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
