using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Template.Objects;

namespace Template.Components.Extensions.Html
{
    public static class TreeViewExtensions
    {
        public static MvcHtmlString TreeViewFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, Tree>> tree)
        {
            return TreeViewFor(String.Format("{0}.{1}", ExpressionHelper.GetExpressionText(tree), "SelectedIds"),
                ModelMetadata.FromLambdaExpression(tree, html.ViewData).Model as Tree);
        }

        private static MvcHtmlString TreeViewFor(String name, Tree tree)
        {
            return new MvcHtmlString(FormIdSpan(name, tree.SelectedIds) + FormTreeView(name, tree.Nodes));
        }
        private static String FormIdSpan(String name, IList<String> selectedIds)
        {
            TagBuilder idSpan = new TagBuilder("span");
            idSpan.AddCssClass("tree-view-ids");

            StringBuilder hiddenInputs = new StringBuilder();
            TagBuilder input = new TagBuilder("input");
            input.MergeAttribute("type", "hidden");
            input.MergeAttribute("name", name);

            foreach (String id in selectedIds)
            {
                input.MergeAttribute("value", id, true);
                hiddenInputs.Append(input.ToString(TagRenderMode.SelfClosing));
            }

            idSpan.InnerHtml = hiddenInputs.ToString();
            return idSpan.ToString();
        }
        private static String FormTreeView(String name, IEnumerable<TreeNode> treeNodes)
        {
            TagBuilder treeView = new TagBuilder("div");
            treeView.MergeAttribute("for", name);
            treeView.AddCssClass("tree-view");

            AddNodes(treeView, treeNodes);
            return treeView.ToString();
        }
        private static void AddNodes(TagBuilder root, IEnumerable<TreeNode> nodes)
        {
            if (nodes.Count() == 0) return;

            TagBuilder branch = new TagBuilder("ul");
            StringBuilder leafBuilder = new StringBuilder();

            foreach (TreeNode treeNode in nodes)
            {
                TagBuilder node = new TagBuilder("li");
                node.MergeAttribute("id", treeNode.Id);
                node.InnerHtml = treeNode.Name;

                AddNodes(node, treeNode.Nodes);
                leafBuilder.Append(node);
            }

            branch.InnerHtml = leafBuilder.ToString();
            root.InnerHtml += branch.ToString();
        }
    }
}
