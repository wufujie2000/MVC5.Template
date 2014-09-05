using MvcTemplate.Components.Extensions.Html;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
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

        protected BaseView()
        {
            DateTime now = DateTime.Now;
            EntityDate = new DateTime(now.Ticks / 100000 * 100000, now.Kind);
        }
    }
}
