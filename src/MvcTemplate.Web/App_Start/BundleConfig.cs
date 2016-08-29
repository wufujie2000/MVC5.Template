using System.Web.Optimization;

namespace MvcTemplate.Web
{
    public class BundleConfig : IBundleConfig
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);
            RegisterStyles(bundles);
        }
        private void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/JQuery/Bundle").IncludeDirectory("~/Scripts/JQuery", "*.js", true));
            bundles.Add(new ScriptBundle("~/Scripts/Bootstrap/Bundle").Include("~/Scripts/Bootstrap/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/JQueryUI/Bundle").IncludeDirectory("~/Scripts/JQueryUI", "*.js", true));
            bundles.Add(new ScriptBundle("~/Scripts/MvcGrid/Bundle").IncludeDirectory("~/Scripts/MvcGrid", "*.js", true));
            bundles.Add(new ScriptBundle("~/Scripts/JsTree/Bundle").Include("~/Scripts/JsTree/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/MvcDatalist/Bundle").IncludeDirectory("~/Scripts/MvcDatalist", "*.js", true));
            bundles.Add(new ScriptBundle("~/Scripts/Shared/Bundle").Include("~/Scripts/Shared/*.js"));
        }
        private void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/JQueryUI/Bundle").Include("~/Content/JQueryUI/*.css"));
            bundles.Add(new StyleBundle("~/Content/Bootstrap/Bundle").Include("~/Content/Bootstrap/*.css"));
            bundles.Add(new StyleBundle("~/Content/FontAwesome/Bundle").Include("~/Content/FontAwesome/*.css"));
            bundles.Add(new StyleBundle("~/Content/MvcGrid/Bundle").Include("~/Content/MvcGrid/*.css"));
            bundles.Add(new StyleBundle("~/Content/JsTree/Bundle").Include("~/Content/JsTree/*.css"));
            bundles.Add(new StyleBundle("~/Content/MvcDatalist/Bundle").Include("~/Content/MvcDatalist/*.css"));
            bundles.Add(new StyleBundle("~/Content/Shared/Bundle").IncludeDirectory("~/Content/Shared", "*.css", true));
        }
    }
}
