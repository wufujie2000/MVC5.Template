using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class JsTreeExtensions
    {
        public static MvcHtmlString JsTreeFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, JsTree>> expression)
        {
            String name = ExpressionHelper.GetExpressionText(expression) + ".SelectedIds";
            JsTree model = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model as JsTree;

            return new MvcHtmlString(HiddenIdsFor(name, model) + JsTreeFor(name, model));
        }

        private static void Add(TagBuilder tree, IList<JsTreeNode> nodes)
        {
            if (nodes.Count == 0) return;

            TagBuilder branch = new TagBuilder("ul");
            StringBuilder nodeBuilder = new StringBuilder();

            foreach (JsTreeNode jsNode in nodes)
            {
                TagBuilder node = new TagBuilder("li");
                String id = jsNode.Id.ToString();
                node.InnerHtml = jsNode.Title;
                node.MergeAttribute("id", id);

                Add(node, jsNode.Nodes);
                nodeBuilder.Append(node);
            }

            branch.InnerHtml = nodeBuilder.ToString();
            tree.InnerHtml += branch.ToString();
        }
        private static String HiddenIdsFor(String name, JsTree model)
        {
            StringBuilder inputs = new StringBuilder();
            TagBuilder input = new TagBuilder("input");
            input.MergeAttribute("type", "hidden");
            input.MergeAttribute("name", name);

            foreach (Int32 id in model.SelectedIds)
            {
                input.MergeAttribute("value", id.ToString(), true);
                inputs.Append(input.ToString(TagRenderMode.SelfClosing));
            }

            TagBuilder ids = new TagBuilder("div");
            ids.AddCssClass("js-tree-view-ids");
            ids.InnerHtml = inputs.ToString();

            return ids.ToString();
        }
        private static String JsTreeFor(String name, JsTree model)
        {
            TagBuilder tree = new TagBuilder("div");
            tree.MergeAttribute("for", name);
            tree.AddCssClass("js-tree-view");

            Add(tree, model.Nodes);

            return tree.ToString();
        }
    }
}
