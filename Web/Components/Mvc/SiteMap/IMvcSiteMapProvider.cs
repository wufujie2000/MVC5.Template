namespace Template.Components.Mvc.SiteMap
{
    public interface IMvcSiteMapProvider
    {
        MvcSiteMapNodeCollection GetMenus();
        MvcSiteMapNodeCollection GenerateBreadcrumb();
    }
}
