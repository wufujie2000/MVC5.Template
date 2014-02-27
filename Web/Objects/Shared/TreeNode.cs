using System;
using System.Collections.Generic;

namespace Template.Objects
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
            : this(String.Empty, name)
        {
        }
        public TreeNode()
            : this(String.Empty, String.Empty)
        {
        }
    }
}