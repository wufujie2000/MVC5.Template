using NUnit.Framework;
using Template.Web;

namespace Template.Tests.Tests.Web.App_Start
{
    [TestFixture]
    public class ViewEngineTests
    {
        private ViewEngine viewEngine;

        [SetUp]
        public void TestInit()
        {
            viewEngine = new ViewEngine();
        }

        #region Constructor: ViewEngine()

        [Test]
        public void ViewEngine_SetsAreaViewLocationFormats()
        {
            var expected = new[] { "~/Views/{2}/{1}/{0}.cshtml" };
            var actual = viewEngine.AreaViewLocationFormats;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ViewEngine_SetsAreaMasterLocationFormats()
        {
            var expected = new[] { "~/Views/{2}/{1}/{0}.cshtml" };
            var actual = viewEngine.AreaMasterLocationFormats;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ViewEngine_SetsAreaPartialViewLocationFormats()
        {
            var expected = new[] { "~/Views/{2}/{1}/{0}.cshtml" };
            var actual = viewEngine.AreaPartialViewLocationFormats;

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion
    }
}
