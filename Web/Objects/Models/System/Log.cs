using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class Log : BaseModel
    {
        public String AccountId { get; set; }

        [Required]
        public String Message { get; set; }

        public Account Account { get; set; }

        public Log()
        {
        }
        public Log(String accountId, String message)
        {
            AccountId = accountId;
            Message = message;
        }
    }
}
