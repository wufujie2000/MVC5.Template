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
                if (id == null)
                    id = Guid.NewGuid().ToString();
                return id;
            }
            set
            {
                id = value;
            }
        }
    }
}
