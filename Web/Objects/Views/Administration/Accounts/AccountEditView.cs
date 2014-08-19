using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountEditView : BaseView
    {
        [ReadOnly(true)]
        public String Username { get; set; }

        public String RoleId { get; set; }
        public String RoleName { get; set; }
    }
}
