using MvcTemplate.Components.Extensions;
using NSubstitute;
using System;
using System.IO;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions
{
    public class ContentExtensionsTests
    {
        private String path;
        private HtmlHelper html;
        private UrlHelper urlHelper;

        public ContentExtensionsTests()
        {
            path = "ContentExtensionsTests.txt";
            File.WriteAllText(path, "");

            html = HtmlHelperFactory.CreateHtmlHelper();
            urlHelper = new UrlHelper(html.ViewContext.RequestContext);
        }

        #region RenderControllerScript(this HtmlHelper html)

        [Fact]
        public void RenderControllerScript_WithArea()
        {
            String source = urlHelper.Content("~/Scripts/Shared/administration/accounts/accounts.js");
            html.ViewContext.HttpContext.Server.MapPath(source).Returns(path);

            String expected = String.Format("<script src=\"{0}?v{1}\"></script>", source, ContentExtensions.Version);
            String actual = html.RenderControllerScript().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerScript_WithoutArea()
        {
            String source = urlHelper.Content("~/Scripts/Shared/accounts/accounts.js");
            html.ViewContext.HttpContext.Server.MapPath(source).Returns(path);
            html.ViewContext.RouteData.Values["Area"] = null;

            String expected = String.Format("<script src=\"{0}?v{1}\"></script>", source, ContentExtensions.Version);
            String actual = html.RenderControllerScript().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerScript_CachesResultPath()
        {
            String source = urlHelper.Content("~/Scripts/Shared/administration/accounts/accounts.js");
            html.ViewContext.HttpContext.Server.MapPath(source).Returns(path);

            html.RenderControllerScript();
            html.ViewContext.HttpContext.Server.MapPath(source).Returns("Test");

            String expected = String.Format("<script src=\"{0}?v{1}\"></script>", source, ContentExtensions.Version);
            String actual = html.RenderControllerScript().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerScript_NoScripts_DoesNotRender()
        {
            html.ViewContext.HttpContext.Server.MapPath(Arg.Any<String>()).Returns("");
            html.ViewContext.RouteData.Values["Controller"] = "Test";

            String actual = html.RenderControllerScript().ToString();
            String expected = "";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region RenderControllerStyle(this HtmlHelper html)

        [Fact]
        public void RenderControllerStyle_WithArea()
        {
            String source = urlHelper.Content("~/Content/Shared/administration/accounts/accounts.css");
            html.ViewContext.HttpContext.Server.MapPath(source).Returns(path);

            String expected = String.Format("<link href=\"{0}?v{1}\" rel=\"stylesheet\"></link>", source, ContentExtensions.Version);
            String actual = html.RenderControllerStyle().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerStyle_WithoutArea()
        {
            String source = urlHelper.Content("~/Content/Shared/accounts/accounts.css");
            html.ViewContext.HttpContext.Server.MapPath(source).Returns(path);
            html.ViewContext.RouteData.Values["Area"] = null;

            String expected = String.Format("<link href=\"{0}?v{1}\" rel=\"stylesheet\"></link>", source, ContentExtensions.Version);
            String actual = html.RenderControllerStyle().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerStyle_CachesResultPath()
        {
            String source = urlHelper.Content("~/Content/Shared/administration/accounts/accounts.css");
            html.ViewContext.HttpContext.Server.MapPath(source).Returns(path);

            html.RenderControllerStyle();
            html.ViewContext.HttpContext.Server.MapPath(source).Returns("Test");

            String expected = String.Format("<link href=\"{0}?v{1}\" rel=\"stylesheet\"></link>", source, ContentExtensions.Version);
            String actual = html.RenderControllerStyle().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RenderControllerStyle_NoStyles_DoesNotRender()
        {
            html.ViewContext.HttpContext.Server.MapPath(Arg.Any<String>()).Returns("");
            html.ViewContext.RouteData.Values["Controller"] = "Test";

            String actual = html.RenderControllerStyle().ToString();
            String expected = "";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
