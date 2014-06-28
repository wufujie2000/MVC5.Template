using System;

namespace MvcTemplate.Components.Mvc
{
    public class MvcSiteMapMenuNode
    {
        public String Title { get; set; }
        public String IconClass { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean HasActiveSubMenu { get; set; }

        public String Area { get; set; }
        public String Action { get; set; }
        public String Controller { get; set; }

        public MvcSiteMapMenuNode Parent { get; set; }
        public MvcSiteMapMenuCollection Submenus { get; set; }
    }
}
