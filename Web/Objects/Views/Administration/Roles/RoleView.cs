using Datalist;
using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class RoleView : BaseView
    {
        [Required]
        [DatalistColumn()]
        public String Name { get; set; }

        public Tree PrivilegesTree { get; set; }

        public RoleView()
        {
            PrivilegesTree = new Tree();
        }
    }
}
