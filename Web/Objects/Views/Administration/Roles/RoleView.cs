using Datalist;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class RoleView : BaseView
    {
        [Required]
        [DatalistColumn()]
        public String Name { get; set; }

        public Tree PrivilegesTree { get; set; }
        public IList<RolePrivilegeView> RolePrivileges { get; set; }

        public RoleView()
        {
            PrivilegesTree = new Tree();
            RolePrivileges = new List<RolePrivilegeView>();
        }
    }
}
