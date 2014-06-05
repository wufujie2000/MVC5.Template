using System;
using System.Collections.Generic;

namespace Template.Components.Mvc.SiteMap
{
    public class MvcSiteMapNode
    {
        public String Title { get; set; }
        public Boolean IsMenu { get; set; }

        public String Area { get; set; }
        public String Controller { get; set; }
        public String Action { get; set; }

        public MvcSiteMapNode Parent { get; set; }
        public IEnumerable<MvcSiteMapNode> Children { get; set; }
    }
}
