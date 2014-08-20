using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MvcTemplate.Objects
{
    public class Log : BaseModel
    {
        public String AccountId { get; set; }

        [Required]
        public String Message { get; set; }

        public Log()
        {
            AccountId = HttpContext.Current.User.Identity.Name;
        }
        public Log(String message)
            : this()
        {
            Message = message;
        }
    }
}
