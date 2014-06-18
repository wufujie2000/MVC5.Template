namespace Template.Components.Mvc
{
    public interface IMvcSiteMapProvider
    {
        MvcSiteMapMenuCollection GetMenus();
        MvcSiteMapBreadcrumb GetBreadcrumb();
    }
}
