using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Page
{
    [TestFixture]
    public class JavascriptExtensionsTests
    {
        private String testFilePath;
        private HtmlHelper html;

        [SetUp]
        public void SetUp()
        {
            testFilePath = "JavascriptExtensionsTests.txt";
            if (!File.Exists(testFilePath))
                File.WriteAllText("JavascriptExtensionsTests.txt", String.Empty);

            html = HtmlHelperFactory.CreateHtmlHelper();
        }

        #region Extension method: RenderControllerScripts(this HtmlHelper html)

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithArea()
        {
            html.ViewContext.HttpContext.Server.MapPath("/Scripts/Shared/Area/Controller/controller.js").Returns(testFilePath);

            String expected = "<script src=\"/Scripts/Shared/Area/Controller/controller.js\"></script>";
            String actual = html.RenderControllerScripts().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithAreaOnSpecificDomain()
        {
            html.ViewContext.HttpContext.Request.ApplicationPath.Returns("/TestDomain");
            html.ViewContext.HttpContext.Server.MapPath("/TestDomain/Scripts/Shared/Area/Controller/controller.js").Returns(testFilePath);

            String expected = "<script src=\"/TestDomain/Scripts/Shared/Area/Controller/controller.js\"></script>";
            String actual = html.RenderControllerScripts().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithoutArea()
        {
            html.ViewContext.RouteData.Values["Area"] = null;
            html.ViewContext.HttpContext.Server.MapPath("/Scripts/Shared/Controller/controller.js").Returns(testFilePath);

            String expected = "<script src=\"/Scripts/Shared/Controller/controller.js\"></script>";
            String actual = html.RenderControllerScripts().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithoutAreaOnSpecificDomain()
        {
            html.ViewContext.RouteData.Values["Area"] = null;
            html.ViewContext.HttpContext.Request.ApplicationPath.Returns("/TestDomain");
            html.ViewContext.HttpContext.Server.MapPath("/TestDomain/Scripts/Shared/Controller/controller.js").Returns(testFilePath);

            String expected = "<script src=\"/TestDomain/Scripts/Shared/Controller/controller.js\"></script>";
            String actual = html.RenderControllerScripts().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RenderControllerScripts_DoesNotRendersNotExistingScripts()
        {
            html.ViewContext.HttpContext.Server.MapPath(Arg.Any<String>()).Returns(String.Empty);

            String actual = html.RenderControllerScripts().ToString();
            String expected = String.Empty;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
