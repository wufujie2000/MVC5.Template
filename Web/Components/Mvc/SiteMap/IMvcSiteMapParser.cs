using System.Collections.Generic;
using System.Xml.Linq;

namespace MvcTemplate.Components.Mvc
{
    public interface IMvcSiteMapParser
    {
        IEnumerable<MvcSiteMapNode> GetAllNodes(XElement siteMap);
        IEnumerable<MvcSiteMapNode> GetMenuNodes(XElement siteMap);
    }
}
