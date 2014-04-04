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
            var jQueryScripts = new ScriptBundle("~/Scripts/JQuery");
            jQueryScripts.Include("~/Scripts/JQuery/*.js");

            var sidebarScripts = new ScriptBundle("~/Scripts/Sidebar");
            sidebarScripts.Include("~/Scripts/Sidebar/*.js");

            var bootstrapScripts = new ScriptBundle("~/Scripts/Bootstrap");
            bootstrapScripts.Include("~/Scripts/Bootstrap/*.js");

            var jQueryUIScripts = new ScriptBundle("~/Scripts/JQueryUI");
            jQueryUIScripts.Include("~/Scripts/JQueryUI/*.js");

            var globalizeScripts = new ScriptBundle("~/Scripts/Globalize");
            globalizeScripts.Include("~/Scripts/Globalize/*.js");

            var gridMvcScripts = new ScriptBundle("~/Scripts/GridMvc");
            gridMvcScripts.Include("~/Scripts/GridMvc/*.js");

            var select2Scripts = new ScriptBundle("~/Scripts/Select2");
            select2Scripts.Include("~/Scripts/Select2/*.js");

            var jsTreeScripts = new ScriptBundle("~/Scripts/JsTree");
            jsTreeScripts.Include("~/Scripts/JsTree/*.js");

            var datalistScritps = new ScriptBundle("~/Scripts/Datalist");
            datalistScritps.Include("~/Scripts/Datalist/*.js");

            var loginScripts = new ScriptBundle("~/Scripts/Login");
            loginScripts.Include("~/Scripts/Login/*.js");

            var sharedScripts = new ScriptBundle("~/Scripts/Shared");
            sharedScripts.Include("~/Scripts/Shared/*.js");
            
            bundles.Add(jQueryScripts);
            bundles.Add(sidebarScripts);
            bundles.Add(bootstrapScripts);
            bundles.Add(jQueryUIScripts);
            bundles.Add(globalizeScripts);
            bundles.Add(gridMvcScripts);
            bundles.Add(select2Scripts);
            bundles.Add(jsTreeScripts);
            bundles.Add(datalistScritps);
            bundles.Add(loginScripts);
            bundles.Add(sharedScripts);
        }
        private static void RegisterCss(BundleCollection bundles)
        {
            var jQueryUIStyles = new StyleBundle("~/Content/Css/JQueryUI");
            jQueryUIStyles.Include("~/Content/Css/JQueryUI/*.css");

            var bootstrapStyles = new StyleBundle("~/Content/Css/Bootstrap");
            bootstrapStyles.Include("~/Content/Css/Bootstrap/*.css");

            var fontAwesomeStyles = new StyleBundle("~/Content/Css/FontAwesome");
            fontAwesomeStyles.Include("~/Content/Css/FontAwesome/*.css");

            var gridmvcStyles = new StyleBundle("~/Content/Css/GridMvc");
            gridmvcStyles.Include("~/Content/Css/GridMvc/*.css");

            var select2Styles = new StyleBundle("~/Content/Css/Select2");
            select2Styles.Include("~/Content/Css/Select2/*.css");

            var jsTreeStyles = new StyleBundle("~/Content/Css/JsTree");
            jsTreeStyles.Include("~/Content/Css/JsTree/*.css");

            var datalistStyles = new StyleBundle("~/Content/Css/Datalist");
            datalistStyles.Include("~/Content/Css/Datalist/*.css");

            var loginStyles = new StyleBundle("~/Content/Css/Login");
            loginStyles.Include("~/Content/Css/Login/*.css");

            var sharedStyles = new StyleBundle("~/Content/Css/Shared");
            sharedStyles.Include("~/Content/Css/Shared/*.css");

            bundles.Add(jQueryUIStyles);
            bundles.Add(bootstrapStyles);
            bundles.Add(fontAwesomeStyles);
            bundles.Add(gridmvcStyles);
            bundles.Add(select2Styles);
            bundles.Add(jsTreeStyles);
            bundles.Add(datalistStyles);
            bundles.Add(loginStyles);
            bundles.Add(sharedStyles);
        }
    }
}