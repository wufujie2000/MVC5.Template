using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace MvcTemplate.Objects
{
    public class Log : BaseModel
    {
        [Index]
        [StringLength(128)]
        public String AccountId { get; set; }

        [Required]
        public String Message { get; set; }

        public Log()
        {
        }
        public Log(String message)
        {
            AccountId = HttpContext.Current.User.Identity.Name;
            Message = message;
        }
    }
}
