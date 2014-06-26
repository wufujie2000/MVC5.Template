using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class AccountEditView : BaseView
    {
        public String Username { get; set; }
        public String Email { get; set; }

        public String RoleId { get; set; }
        public String RoleName { get; set; }
    }
}
