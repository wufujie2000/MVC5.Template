using System;

namespace Template.Components.Mvc.SiteMap
{
    public class MvcSiteMapNode
    {
        public String Title { get; set; }
        public Boolean IsMenu { get; set; }
        public String IconClass { get; set; }

        public String Area { get; set; }
        public String Controller { get; set; }
        public String Action { get; set; }

        public MvcSiteMapNode Parent { get; set; }
        public MvcSiteMapMenuCollection Children { get; set; }
    }
}
