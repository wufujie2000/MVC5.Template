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
            ScriptBundle jQueryScripts = new ScriptBundle("~/Scripts/JQuery/Bundle");
            jQueryScripts.Include("~/Scripts/JQuery/*.js");

            ScriptBundle sidebarScripts = new ScriptBundle("~/Scripts/Sidebar/Bundle");
            sidebarScripts.Include("~/Scripts/Sidebar/*.js");

            ScriptBundle bootstrapScripts = new ScriptBundle("~/Scripts/Bootstrap/Bundle");
            bootstrapScripts.Include("~/Scripts/Bootstrap/*.js");

            ScriptBundle jQueryUIScripts = new ScriptBundle("~/Scripts/JQueryUI/Bundle");
            jQueryUIScripts.Include("~/Scripts/JQueryUI/*.js");

            ScriptBundle globalizeScripts = new ScriptBundle("~/Scripts/Globalize/Bundle");
            globalizeScripts.Include("~/Scripts/Globalize/*.js");

            ScriptBundle gridMvcScripts = new ScriptBundle("~/Scripts/GridMvc/Bundle");
            gridMvcScripts.Include("~/Scripts/GridMvc/*.js");

            ScriptBundle select2Scripts = new ScriptBundle("~/Scripts/Select2/Bundle");
            select2Scripts.Include("~/Scripts/Select2/*.js");

            ScriptBundle jsTreeScripts = new ScriptBundle("~/Scripts/JsTree/Bundle");
            jsTreeScripts.Include("~/Scripts/JsTree/*.js");

            ScriptBundle datalistScritps = new ScriptBundle("~/Scripts/Datalist/Bundle");
            datalistScritps.Include("~/Scripts/Datalist/*.js");

            ScriptBundle loginScripts = new ScriptBundle("~/Scripts/Login/Bundle");
            loginScripts.Include("~/Scripts/Login/*.js");

            ScriptBundle sharedScripts = new ScriptBundle("~/Scripts/Shared/Bundle");
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
            StyleBundle jQueryUIStyles = new StyleBundle("~/Content/JQueryUI/Bundle");
            jQueryUIStyles.Include("~/Content/JQueryUI/*.css");

            StyleBundle bootstrapStyles = new StyleBundle("~/Content/Bootstrap/Bundle");
            bootstrapStyles.Include("~/Content/Bootstrap/*.css");

            StyleBundle fontAwesomeStyles = new StyleBundle("~/Content/FontAwesome/Bundle");
            fontAwesomeStyles.Include("~/Content/FontAwesome/*.css");

            StyleBundle gridmvcStyles = new StyleBundle("~/Content/GridMvc/Bundle");
            gridmvcStyles.Include("~/Content/GridMvc/*.css");

            StyleBundle select2Styles = new StyleBundle("~/Content/Select2/Bundle");
            select2Styles.Include("~/Content/Select2/*.css");

            StyleBundle jsTreeStyles = new StyleBundle("~/Content/JsTree/Bundle");
            jsTreeStyles.Include("~/Content/JsTree/*.css");

            StyleBundle datalistStyles = new StyleBundle("~/Content/Datalist/Bundle");
            datalistStyles.Include("~/Content/Datalist/*.css");

            StyleBundle loginStyles = new StyleBundle("~/Content/Login/Bundle");
            loginStyles.Include("~/Content/Login/*.css");

            StyleBundle sharedStyles = new StyleBundle("~/Content/Shared/Bundle");
            sharedStyles.Include("~/Content/Shared/*.css");

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