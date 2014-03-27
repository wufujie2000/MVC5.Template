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
            var jQueryCss = new StyleBundle("~/Content/Css/JQuery");
            jQueryCss.Include("~/Content/Css/JQuery/*.css");

            var bootstrapCss = new StyleBundle("~/Content/Css/Bootstrap");
            bootstrapCss.Include("~/Content/Css/Bootstrap/*.css");

            var fontAwesomeCss = new StyleBundle("~/Content/Css/FontAwesome");
            fontAwesomeCss.Include("~/Content/Css/FontAwesome/*.css");

            var gridmvcCss = new StyleBundle("~/Content/Css/GridMvc");
            gridmvcCss.Include("~/Content/Css/GridMvc/*.css");

            var select2Css = new StyleBundle("~/Content/Css/Select2");
            select2Css.Include("~/Content/Css/Select2/*.css");

            var jsTreeCss = new StyleBundle("~/Content/Css/JsTree");
            jsTreeCss.Include("~/Content/Css/JsTree/*.css");

            var datalistCss = new StyleBundle("~/Content/Css/Datalist");
            datalistCss.Include("~/Content/Css/Datalist/*.css");

            var loginCss = new StyleBundle("~/Content/Css/Login");
            loginCss.Include("~/Content/Css/Login/*.css");

            var sharedCss = new StyleBundle("~/Content/Css/Shared");
            sharedCss.Include("~/Content/Css/Shared/*.css");

            bundles.Add(jQueryCss);
            bundles.Add(bootstrapCss);
            bundles.Add(fontAwesomeCss);
            bundles.Add(gridmvcCss);
            bundles.Add(select2Css);
            bundles.Add(jsTreeCss);
            bundles.Add(datalistCss);
            bundles.Add(loginCss);
            bundles.Add(sharedCss);
        }
    }
}