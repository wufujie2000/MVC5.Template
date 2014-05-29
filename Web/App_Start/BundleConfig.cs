using System.Web.Optimization;

namespace Template.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);
            RegisterCss(bundles);
        }
        private static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/JQuery/Bundle").Include("~/Scripts/JQuery/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Sidebar/Bundle").Include("~/Scripts/Sidebar/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Bootstrap/Bundle").Include("~/Scripts/Bootstrap/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/JQueryUI/Bundle").Include("~/Scripts/JQueryUI/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Globalize/Bundle").Include("~/Scripts/Globalize/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/GridMvc/Bundle").Include("~/Scripts/GridMvc/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Select2/Bundle").Include("~/Scripts/Select2/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/JsTree/Bundle").Include("~/Scripts/JsTree/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Datalist/Bundle").Include("~/Scripts/Datalist/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Login/Bundle").Include("~/Scripts/Login/*.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Shared/Bundle").Include("~/Scripts/Shared/*.js"));
        }
        private static void RegisterCss(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/JQueryUI/Bundle").Include("~/Content/JQueryUI/*.css"));
            bundles.Add(new StyleBundle("~/Content/Bootstrap/Bundle").Include("~/Content/Bootstrap/*.css"));
            bundles.Add(new StyleBundle("~/Content/FontAwesome/Bundle").Include("~/Content/FontAwesome/*.css"));
            bundles.Add(new StyleBundle("~/Content/GridMvc/Bundle").Include("~/Content/GridMvc/*.css"));
            bundles.Add(new StyleBundle("~/Content/Select2/Bundle").Include("~/Content/Select2/*.css"));
            bundles.Add(new StyleBundle("~/Content/JsTree/Bundle").Include("~/Content/JsTree/*.css"));
            bundles.Add(new StyleBundle("~/Content/Datalist/Bundle").Include("~/Content/Datalist/*.css"));
            bundles.Add(new StyleBundle("~/Content/Login/Bundle").Include("~/Content/Login/*.css"));
            bundles.Add(new StyleBundle("~/Content/Shared/Bundle").Include("~/Content/Shared/*.css"));
        }
    }
}