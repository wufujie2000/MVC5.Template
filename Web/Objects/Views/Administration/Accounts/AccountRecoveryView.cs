using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountRecoveryView : BaseView
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; }
    }
}
