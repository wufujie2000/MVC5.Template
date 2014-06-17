using System;
using System.ComponentModel.DataAnnotations;
using Template.Components.Extensions.Html;

namespace Template.Objects
{
    public abstract class BaseView : ILinkableView
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

        public DateTime? EntityDate
        {
            get;
            private set;
        }

        public BaseView()
        {
            DateTime now = DateTime.Now;
            EntityDate = new DateTime(now.Ticks / 10000 * 10000, now.Kind);
        }
    }
}
