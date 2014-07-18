using Moq;
using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Tests.Helpers;
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
        private HttpMock httpMock;
        private HtmlHelper html;

        [SetUp]
        public void SetUp()
        {
            testFilePath = "JavascriptExtensionsTests.txt";
            if (!File.Exists(testFilePath))
                File.WriteAllText("JavascriptExtensionsTests.txt", String.Empty);

            HtmlMock htmlMock = new HtmlMock();
            httpMock = htmlMock.HttpMock;
            html = htmlMock.Html;
        }

        #region Extension method: RenderControllerScripts(this HtmlHelper html)

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithArea()
        {
            httpMock.HttpServerMock
                .Setup(mock => mock.MapPath("/Scripts/Shared/Area/Controller/controller.js"))
                .Returns(testFilePath);

            String expected = "<script src=\"/Scripts/Shared/Area/Controller/controller.js\"></script>";
            String actual = html.RenderControllerScripts().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithAreaOnSpecificDomain()
        {
            httpMock.HttpRequestMock
                .Setup(mock => mock.ApplicationPath)
                .Returns("/TestDomain");
            httpMock.HttpServerMock
                .Setup(mock => mock.MapPath("/TestDomain/Scripts/Shared/Area/Controller/controller.js"))
                .Returns(testFilePath);

            String expected = "<script src=\"/TestDomain/Scripts/Shared/Area/Controller/controller.js\"></script>";
            String actual = html.RenderControllerScripts().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithoutArea()
        {
            html.ViewContext.RouteData.Values["Area"] = null;
            httpMock.HttpServerMock
                .Setup(mock => mock.MapPath("/Scripts/Shared/Controller/controller.js"))
                .Returns(testFilePath);

            String expected = "<script src=\"/Scripts/Shared/Controller/controller.js\"></script>";
            String actual = html.RenderControllerScripts().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithoutAreaOnSpecificDomain()
        {
            html.ViewContext.RouteData.Values["Area"] = null;
            httpMock.HttpRequestMock
                .Setup(mock => mock.ApplicationPath)
                .Returns("/TestDomain");
            httpMock.HttpServerMock
                .Setup(mock => mock.MapPath("/TestDomain/Scripts/Shared/Controller/controller.js"))
                .Returns(testFilePath);

            String expected = "<script src=\"/TestDomain/Scripts/Shared/Controller/controller.js\"></script>";
            String actual = html.RenderControllerScripts().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RenderControllerScripts_DoesNotRendersNotExistingScripts()
        {
            httpMock.HttpServerMock.Setup(mock => mock.MapPath(It.IsAny<String>())).Returns(String.Empty);

            String actual = html.RenderControllerScripts().ToString();
            String expected = String.Empty;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
