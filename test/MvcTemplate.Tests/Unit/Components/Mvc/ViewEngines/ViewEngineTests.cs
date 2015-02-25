using MvcTemplate.Components.Mvc;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ViewEngineTests
    {
        private ViewEngine viewEngine;

        public ViewEngineTests()
        {
            viewEngine = new ViewEngine();
        }

        #region Constructor: ViewEngine()

        [Fact]
        public void ViewEngine_SetsAreaViewLocationFormats()
        {
            String[] actual = viewEngine.AreaViewLocationFormats;
            String[] expected = { "~/Views/{2}/{1}/{0}.cshtml" };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ViewEngine_SetsAreaMasterLocationFormats()
        {
            String[] actual = viewEngine.AreaMasterLocationFormats;
            String[] expected = { "~/Views/{2}/{1}/{0}.cshtml" };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ViewEngine_SetsAreaPartialViewLocationFormats()
        {
            String[] actual = viewEngine.AreaPartialViewLocationFormats;
            String[] expected = { "~/Views/{2}/{1}/{0}.cshtml" };

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
