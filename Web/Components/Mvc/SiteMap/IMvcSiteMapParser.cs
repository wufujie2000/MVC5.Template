using System.Collections.Generic;
using System.Xml.Linq;

namespace Template.Components.Mvc.SiteMap
{
    public interface IMvcSiteMapParser
    {
        IEnumerable<MvcSiteMapNode> GetAllNodes(XElement siteMap);
        IEnumerable<MvcSiteMapNode> GetMenuNodes(XElement siteMap);
    }
}
