using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Extensions.Html
{
    public class JsTreeNode
    {
        public String Id { get; set; }
        public String Name { get; set; }

        public IList<JsTreeNode> Nodes { get; set; }

        public JsTreeNode(String id, String name)
        {
            Id = id;
            Name = name;
            Nodes = new List<JsTreeNode>();
        }
        public JsTreeNode(String name)
            : this(null, name)
        {
        }
    }
}