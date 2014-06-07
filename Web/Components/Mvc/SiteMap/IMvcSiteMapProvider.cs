namespace Template.Components.Mvc.SiteMap
{
    public interface IMvcSiteMapProvider
    {
        MvcSiteMapMenuCollection GetMenus();
        MvcSiteMapBreadcrumb GetBreadcrumb();
    }
}
