using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountEditView : BaseView
    {
        [Editable(false)]
        public String Username { get; set; }

        public String RoleId { get; set; }
        public String RoleName { get; set; }
    }
}
