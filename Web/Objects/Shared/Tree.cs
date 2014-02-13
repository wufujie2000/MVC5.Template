using System;
using System.Collections.Generic;

namespace Template.Objects
{
    public class Tree
    {
        public IList<TreeNode> Nodes { get; set; }
        public IList<String> SelectedIds { get; set; }

        public Tree()
        {
            Nodes = new List<TreeNode>();
            SelectedIds = new List<String>();
        }
    }
}