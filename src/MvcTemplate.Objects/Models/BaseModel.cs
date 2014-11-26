using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public abstract class BaseModel
    {
        private String id;

        [Key]
        [Required]
        public String Id
        {
            get
            {
                return id ?? (id = Guid.NewGuid().ToString());
            }
            set
            {
                id = value;
            }
        }

        [Required]
        public DateTime CreationDate
        {
            get;
            protected set;
        }

        protected BaseModel()
        {
            DateTime now = DateTime.Now;
            CreationDate = new DateTime(now.Ticks / 100000 * 100000, now.Kind);
        }
    }
}
