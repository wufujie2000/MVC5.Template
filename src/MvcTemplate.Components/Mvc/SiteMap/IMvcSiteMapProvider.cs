using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public interface IMvcSiteMapProvider
    {
        IEnumerable<MvcSiteMapNode> GetAuthorizedMenus();
        IEnumerable<MvcSiteMapNode> GetBreadcrumb();
    }
}
