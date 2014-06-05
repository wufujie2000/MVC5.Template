using System.Collections.Generic;

namespace Template.Components.Mvc.SiteMap
{
    public interface IMvcSiteMapProvider
    {
        IEnumerable<MvcSiteMapNode> GetMenus();
        IEnumerable<MvcSiteMapNode> GenerateBreadcrumb();
    }
}
