using NUnit.Framework;
using System;
using System.Web.Mvc;
using Template.Components.Extensions.Html;
using Template.Objects;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class TreeViewExtensionsTests
    {
        private HtmlHelper<TreeViewModel> html;
        private TreeViewModel model;

        [SetUp]
        public void SetUp()
        {
            model = new TreeViewModel();
            model.Tree.SelectedIds.Add("1");
            model.Tree.Nodes.Add(new TreeNode("Test"));
            model.Tree.Nodes[0].Nodes.Add(new TreeNode("1", "Test1"));
            model.Tree.Nodes[0].Nodes.Add(new TreeNode("2", "Test2"));

            html = new HtmlMock<TreeViewModel>(model).Html;
        }

        #region Extension method: TreeViewFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, Tree>> tree)

        [Test]
        public void TreeViewFor_FormsTreeViewFor()
        {
            

            String actual = html.TreeViewFor(treeModel => treeModel.Tree).ToString();
            String expected = String.Format("<span class=\"tree-view-ids\"><input name=\"Tree.SelectedIds\" type=\"hidden\" value=\"1\" /></span>"
                + "<div class=\"tree-view\" for=\"Tree.SelectedIds\"><ul><li>Test<ul><li id=\"1\">Test1</li><li id=\"2\">Test2</li></ul></li></ul></div>",
                "");

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
