using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public abstract class BaseView
    {
        private String id;

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
    }
}
