using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class JavascriptExtensionsTests
    {
        private UrlHelper urlHelper;
        private HtmlHelper html;
        private String path;

        [SetUp]
        public void SetUp()
        {
            path = "JavascriptExtensionsTests.txt";
            File.WriteAllText(path, String.Empty);

            html = HtmlHelperFactory.CreateHtmlHelper();
            urlHelper = new UrlHelper(html.ViewContext.RequestContext);
        }

        #region Extension method: RenderControllerScripts(this HtmlHelper html)

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithArea()
        {
            String scriptSrc = urlHelper.Content("~/Scripts/Shared/Administration/Accounts/accounts.js");
            html.ViewContext.HttpContext.Server.MapPath(scriptSrc).Returns(path);

            String expected = String.Format("<script src=\"{0}\"></script>", scriptSrc);
            String actual = html.RenderControllerScripts().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RenderControllerScripts_RendersControllerScriptsWithoutArea()
        {
            String scriptSrc = urlHelper.Content("~/Scripts/Shared/Accounts/accounts.js");
            html.ViewContext.HttpContext.Server.MapPath(scriptSrc).Returns(path);
            html.ViewContext.RouteData.Values["Area"] = null;

            String expected = String.Format("<script src=\"{0}\"></script>", scriptSrc);
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
