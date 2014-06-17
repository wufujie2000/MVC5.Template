using System;
using System.Collections.Generic;

namespace Template.Components.Extensions.Html
{
    public class TreeNode
    {
        public String Id { get; set; }
        public String Name { get; set; }

        public IList<TreeNode> Nodes { get; set; }

        public TreeNode(String id, String name)
        {
            Id = id;
            Name = name;
            Nodes = new List<TreeNode>();
        }
        public TreeNode(String name)
            : this(null, name)
        {
        }
        public TreeNode()
            : this(null, null)
        {
        }
    }
}