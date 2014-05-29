using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
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
        public DateTime EntityDate
        {
            get;
            private set;
        }

        public BaseModel()
        {
            DateTime now = DateTime.Now;
            EntityDate = new DateTime(now.Ticks / 10000 * 10000, now.Kind);
        }
    }
}
