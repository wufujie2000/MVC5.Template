using System.Collections.Generic;
using System.Xml.Linq;

namespace Template.Components.Mvc.SiteMap
{
    public interface IMvcSiteMapParser
    {
        IEnumerable<MvcSiteMapNode> GetNodes(XElement siteMap);
        MvcSiteMapMenuCollection GetMenus(XElement siteMap);
    }
}
