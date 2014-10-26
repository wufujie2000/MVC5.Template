namespace MvcTemplate.Components.Mvc
{
    public interface IMvcSiteMapProvider
    {
        MvcSiteMapMenus GetMenus();
        MvcSiteMapBreadcrumb GetBreadcrumb();
    }
}
