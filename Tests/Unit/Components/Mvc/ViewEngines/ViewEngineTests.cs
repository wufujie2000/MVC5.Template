using NUnit.Framework;
using System;
using Template.Components.Mvc;

namespace Template.Tests.Unit.Components.Mvc
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
            String[] expected = new[] { "~/Views/{2}/{1}/{0}.cshtml" };
            String[] actual = viewEngine.AreaViewLocationFormats;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ViewEngine_SetsAreaMasterLocationFormats()
        {
            String[] expected = new[] { "~/Views/{2}/{1}/{0}.cshtml" };
            String[] actual = viewEngine.AreaMasterLocationFormats;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ViewEngine_SetsAreaPartialViewLocationFormats()
        {
            String[] expected = new[] { "~/Views/{2}/{1}/{0}.cshtml" };
            String[] actual = viewEngine.AreaPartialViewLocationFormats;

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion
    }
}
