using NUnit.Framework;
using System;
using System.Text;
using System.Web.Mvc;
using Template.Objects;
using Template.Tests.Helpers;
using Template.Tests.Objects.Components.Extensions.JsTree;
using Template.Components.Extensions.Html;

namespace Template.Tests.Tests.Components.Extensions.Html.JsTree
{
    [TestFixture]
    public class TreeViewExtensionsTests
    {
        private HtmlHelper<TreeViewModel> htmlHelper;
        private TreeViewModel model;

        [SetUp]
        public void SetUp()
        {
            model = new TreeViewModel();
            htmlHelper = new HtmlHelperMock<TreeViewModel>(model).HtmlHelper;
        }

        #region Extension method: TreeViewFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, Tree>> tree)

        [Test]
        public void TreeViewFor_FormsTreeViewFor()
        {
            model.Tree.SelectedIds.Add("1");
            model.Tree.Nodes.Add(new TreeNode("Test"));
            model.Tree.Nodes[0].Nodes.Add(new TreeNode("1", "Test1"));
            model.Tree.Nodes[0].Nodes.Add(new TreeNode("2", "Test2"));

            var idSpan = new TagBuilder("span");
            idSpan.AddCssClass("tree-view-ids");

            var input = new TagBuilder("input");
            input.MergeAttribute("name", "Tree.SelectedIds");
            input.MergeAttribute("type", "hidden");
            input.MergeAttribute("value", "1", true);
            idSpan.InnerHtml += input.ToString(TagRenderMode.SelfClosing);

            var treeView = new TagBuilder("div");
            treeView.MergeAttribute("for", "Tree.SelectedIds");
            treeView.AddCssClass("tree-view");

            TagBuilder root = new TagBuilder("ul");
            TagBuilder node = new TagBuilder("li");
            node.MergeAttribute("id", String.Empty);
            node.InnerHtml = model.Tree.Nodes[0].Name;

            var branch = new TagBuilder("ul");

            var node1 = new TagBuilder("li");
            node1.MergeAttribute("id", "1");
            node1.InnerHtml = model.Tree.Nodes[0].Nodes[0].Name;

            var node2 = new TagBuilder("li");
            node2.MergeAttribute("id", "2");
            node2.InnerHtml = model.Tree.Nodes[0].Nodes[1].Name;

            branch.InnerHtml = node1.ToString() + node2.ToString();
            node.InnerHtml += branch.ToString();
            root.InnerHtml += node.ToString();
            treeView.InnerHtml = root.ToString();

            var expected = idSpan.ToString() + treeView.ToString();
            var actual = htmlHelper.TreeViewFor(treeModel => treeModel.Tree).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
