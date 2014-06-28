namespace MvcTemplate.Components.Mvc
{
    public interface IMvcSiteMapProvider
    {
        MvcSiteMapMenuCollection GetMenus();
        MvcSiteMapBreadcrumb GetBreadcrumb();
    }
}
