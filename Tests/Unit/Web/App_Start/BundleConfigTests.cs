using MvcTemplate.Web;
using NUnit.Framework;
using System;
using System.Web.Optimization;

namespace MvcTemplate.Tests.Unit.Web
{
    [TestFixture]
    public class BundleConfigTests
    {
        #region Static method: RegisterBundles(BundleCollection bundles)

        [Test]
        public void RegisterBundles_RegistersScriptBundles()
        {
            String[] expectedBundles =
            {
                "~/Scripts/JQuery/Bundle",
                "~/Scripts/Bootstrap/Bundle",
                "~/Scripts/JQueryUI/Bundle",
                "~/Scripts/Globalize/Bundle",
                "~/Scripts/GridMvc/Bundle",
                "~/Scripts/JsTree/Bundle",
                "~/Scripts/Datalist/Bundle",
                "~/Scripts/Shared/Bundle"
            };

            BundleCollection bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);
            Type expected = typeof(ScriptBundle);

            foreach (String path in expectedBundles)
                Assert.IsInstanceOf(expected, bundles.GetBundleFor(path));
        }

        [Test]
        public void RegisterBundles_RegistersStyleBundles()
        {
            String[] expectedBundles =
            {
                "~/Content/Bootstrap/Bundle",
                "~/Content/JQueryUI/Bundle",
                "~/Content/FontAwesome/Bundle",
                "~/Content/GridMvc/Bundle",
                "~/Content/JsTree/Bundle",
                "~/Content/Datalist/Bundle",
                "~/Content/Auth/Bundle",
                "~/Content/Shared/Bundle"
            };

            BundleCollection bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);
            Type expected = typeof(StyleBundle);

            foreach (String path in expectedBundles)
                Assert.IsInstanceOf(expected, bundles.GetBundleFor(path));
        }

        #endregion
    }
}
