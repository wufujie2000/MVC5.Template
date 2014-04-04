using NUnit.Framework;
using System;
using System.Web.Optimization;
using Template.Web;

namespace Template.Tests.Unit.Web.App_Start
{
    [TestFixture]
    public class BundleConfigTests
    {
        #region Static method: RegisterBundles(BundleCollection bundles)

        [Test]
        public void RegisterBundles_RegistersScriptBundles()
        {
            var expectedBundles = new[]
            {
                "~/Scripts/JQuery",
                "~/Scripts/Sidebar",
                "~/Scripts/Bootstrap",
                "~/Scripts/JQueryUI",
                "~/Scripts/Globalize",
                "~/Scripts/GridMvc",
                "~/Scripts/Select2",
                "~/Scripts/JsTree",
                "~/Scripts/Datalist",
                "~/Scripts/Login",
                "~/Scripts/Shared"
            };

            var bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);

            foreach (var expectedPath in expectedBundles)
                Assert.IsInstanceOf(typeof(ScriptBundle), bundles.GetBundleFor(expectedPath));
        }

        [Test]
        public void RegisterBundles_RegistersStyleBundles()
        {
            var expectedBundles = new[]
            {
                "~/Content/Css/Bootstrap",
                "~/Content/Css/JQueryUI",
                "~/Content/Css/FontAwesome",
                "~/Content/Css/GridMvc",
                "~/Content/Css/Select2",
                "~/Content/Css/JsTree",
                "~/Content/Css/Datalist",
                "~/Content/Css/Login",
                "~/Content/Css/Shared"
            };
            // TODO: Test for actual search patterns
            var bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);

            foreach (var expectedPath in expectedBundles)
                Assert.IsInstanceOf(typeof(StyleBundle), bundles.GetBundleFor(expectedPath));
        }

        #endregion
    }
}
