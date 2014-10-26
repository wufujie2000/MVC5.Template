using System;

namespace MvcTemplate.Components.Mvc
{
    public class MvcSiteMapBreadcrumbNode
    {
        public String IconClass { get; set; }
        public String Title { get; set; }

        public String Controller { get; set; }
        public String Action { get; set; }
        public String Area { get; set; }
    }
}
