using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class JsTreeViewExtensionsTests
    {
        private HtmlHelper<JsTreeView> html;

        [SetUp]
        public void SetUp()
        {
            JsTreeView model = new JsTreeView();

            model.JsTree.SelectedIds.Add("1");
            model.JsTree.Nodes.Add(new JsTreeNode("Test"));
            model.JsTree.Nodes[0].Nodes.Add(new JsTreeNode("1", "Test1"));
            model.JsTree.Nodes[0].Nodes.Add(new JsTreeNode("2", "Test2"));

            html = new HtmlMock<JsTreeView>(model).Html;
        }

        #region Extension method: JsTreeViewFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, JsTree>> model)

        [Test]
        public void JsTreeViewFor_FormsJsTreeViewFor()
        {
            String actual = html.JsTreeViewFor(model => model.JsTree).ToString();
            String expected = "<span class=\"js-tree-view-ids\"><input name=\"JsTree.SelectedIds\" type=\"hidden\" value=\"1\" /></span>"
                + "<div class=\"js-tree-view\" for=\"JsTree.SelectedIds\"><ul><li>Test<ul><li id=\"1\">Test1</li><li id=\"2\">Test2</li></ul></li></ul></div>";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
