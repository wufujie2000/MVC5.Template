using Datalist;
using System;
using System.ComponentModel.DataAnnotations;
using Template.Components.Extensions.Html;

namespace Template.Objects
{
    public class RoleView : BaseView
    {
        [Required]
        [DatalistColumn()]
        public String Name { get; set; }

        public JsTree PrivilegesTree { get; set; }

        public RoleView()
        {
            PrivilegesTree = new JsTree();
        }
    }
}
