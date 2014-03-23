using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class Log : BaseModel
    {
        [Required]
        public String Message { get; set; }

        public Log()
        {
        }
        public Log(String message)
        {
            Message = message;
        }
    }
}
