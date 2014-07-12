using MvcTemplate.Web;
using NUnit.Framework;
using System;
using System.Web.Optimization;

namespace MvcTemplate.Tests.Unit.Web.App_Start
{
    [TestFixture]
    public class BundleConfigTests
    {
        #region Static method: RegisterBundles(BundleCollection bundles)

        [Test]
        public void RegisterBundles_RegistersScriptBundles()
        {
            String[] expectedBundles = new[]
            {
                "~/Scripts/JQuery/Bundle",
                "~/Scripts/Bootstrap/Bundle",
                "~/Scripts/JQueryUI/Bundle",
                "~/Scripts/Globalize/Bundle",
                "~/Scripts/GridMvc/Bundle",
                "~/Scripts/Select2/Bundle",
                "~/Scripts/JsTree/Bundle",
                "~/Scripts/Datalist/Bundle",
                "~/Scripts/Shared/Bundle"
            };

            BundleCollection bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);

            foreach (String expectedPath in expectedBundles)
                Assert.IsInstanceOf(typeof(ScriptBundle), bundles.GetBundleFor(expectedPath));
        }

        [Test]
        public void RegisterBundles_RegistersStyleBundles()
        {
            String[] expectedBundles = new[]
            {
                "~/Content/Bootstrap/Bundle",
                "~/Content/JQueryUI/Bundle",
                "~/Content/FontAwesome/Bundle",
                "~/Content/GridMvc/Bundle",
                "~/Content/Select2/Bundle",
                "~/Content/JsTree/Bundle",
                "~/Content/Datalist/Bundle",
                "~/Content/Auth/Bundle",
                "~/Content/Shared/Bundle"
            };

            BundleCollection bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);

            foreach (String expectedPath in expectedBundles)
                Assert.IsInstanceOf(typeof(StyleBundle), bundles.GetBundleFor(expectedPath));
        }

        #endregion
    }
}
