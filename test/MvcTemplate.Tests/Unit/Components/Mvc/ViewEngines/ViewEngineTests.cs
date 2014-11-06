using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System;

namespace MvcTemplate.Tests.Unit.Components.Mvc
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
            String[] actual = viewEngine.AreaViewLocationFormats;
            String[] expected = { "~/Views/{2}/{1}/{0}.cshtml" };

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ViewEngine_SetsAreaMasterLocationFormats()
        {
            String[] actual = viewEngine.AreaMasterLocationFormats;
            String[] expected = { "~/Views/{2}/{1}/{0}.cshtml" };

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ViewEngine_SetsAreaPartialViewLocationFormats()
        {
            String[] actual = viewEngine.AreaPartialViewLocationFormats;
            String[] expected = { "~/Views/{2}/{1}/{0}.cshtml" };

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
