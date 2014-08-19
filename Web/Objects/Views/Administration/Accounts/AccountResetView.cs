using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountResetView : BaseView
    {
        [Required]
        public String Token { get; set; }

        [Required]
        public String NewPassword { get; set; }
    }
}
