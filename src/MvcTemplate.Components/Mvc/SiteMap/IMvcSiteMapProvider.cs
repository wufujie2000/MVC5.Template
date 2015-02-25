using System.Collections.Generic;
using System.Web.Routing;

namespace MvcTemplate.Components.Mvc
{
    public interface IMvcSiteMapProvider
    {
        IEnumerable<MvcSiteMapNode> GetAuthorizedMenus(RequestContext request);
        IEnumerable<MvcSiteMapNode> GetBreadcrumb(RequestContext request);
    }
}
