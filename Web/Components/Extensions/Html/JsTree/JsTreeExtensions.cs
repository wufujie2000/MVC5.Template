using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class JsTreeExtensions
    {
        public static MvcHtmlString JsTreeFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, JsTree>> expression)
        {
            return JsTreeFor(String.Format("{0}.{1}", ExpressionHelper.GetExpressionText(expression), "SelectedIds"),
                ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model as JsTree);
        }

        private static MvcHtmlString JsTreeFor(String name, JsTree jsTree)
        {
            return new MvcHtmlString(FormIdSpan(name, jsTree.SelectedIds) + FormTree(name, jsTree.Nodes));
        }
        private static String FormIdSpan(String name, IList<String> selectedIds)
        {
            TagBuilder idSpan = new TagBuilder("span");
            idSpan.AddCssClass("js-tree-view-ids");

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
        private static String FormTree(String name, IEnumerable<JsTreeNode> nodes)
        {
            TagBuilder tree = new TagBuilder("div");
            tree.MergeAttribute("for", name);
            tree.AddCssClass("js-tree-view");

            AddNodes(tree, nodes);

            return tree.ToString();
        }
        private static void AddNodes(TagBuilder root, IEnumerable<JsTreeNode> nodes)
        {
            if (nodes.Count() == 0) return;

            StringBuilder leafBuilder = new StringBuilder();
            TagBuilder branch = new TagBuilder("ul");

            foreach (JsTreeNode treeNode in nodes)
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
