using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class AuthExtensionsTests
    {
        private Expression<Func<BootstrapModel, String>> expression;
        private HtmlHelper<BootstrapModel> html;
        private BootstrapModel model;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            model = new BootstrapModel();
            html = HtmlHelperFactory.CreateHtmlHelper(model);
            expression = (expModel) => expModel.Relation.Required;
        }

        [SetUp]
        public void SetUp()
        {
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            GlobalizationManager.Provider = null;
        }

        #region Extension method: AuthUsernameFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression, Boolean autocomplete = true)

        [Test]
        [TestCase(true, "on")]
        [TestCase(false, "off")]
        public void AuthUsernameFor_FormsUsernameInput(Boolean autocomplete, String htmlAutocomplete)
        {
            String actual = html.AuthUsernameFor(expression, autocomplete).ToString();
            String expected = String.Format(
                "<span class=\"fa fa-user\"></span>" +
                "<input autocomplete=\"{0}\" id=\"{1}\" name=\"{2}\" placeholder=\"\" type=\"text\" value=\"{3}\" />",
                htmlAutocomplete, TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.Required);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthPasswordFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression, Boolean autocomplete = true)

        [Test]
        [TestCase(true, "on")]
        [TestCase(false, "off")]
        public void AuthPasswordFor_FormsPasswordInput(Boolean autocomplete, String htmlAutocomplete)
        {
            String actual = html.AuthPasswordFor(expression, autocomplete).ToString();
            String expected = String.Format(
                "<span class=\"fa fa-lock\"></span>" +
                "<input autocomplete=\"{0}\" id=\"{1}\" name=\"{2}\" placeholder=\"\" type=\"password\" />",
                htmlAutocomplete, TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthEmailFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)

        [Test]
        public void AuthEmailFor_FormsNotAutocompletableEmailInput()
        {
            String actual = html.AuthEmailFor(expression).ToString();
            String expected = String.Format(
                "<span class=\"fa fa-envelope\"></span>" +
                "<input autocomplete=\"off\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.Required);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthLanguageSelect(this HtmlHelper html)

        [Test]
        public void AuthLanguageSelect_OnSingleLanguageReturnsEmptyHtml()
        {
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
            GlobalizationManager.Provider.Languages.Returns(new[] { new Language() });

            String actual = html.AuthLanguageSelect().ToString();
            String expected = "";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuthLanguageSelect_FormsLanguageSelectInput()
        {
            RouteValueDictionary routeValues = html.ViewContext.RouteData.Values;
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            String action = routeValues["action"].ToString();

            String actual = html.AuthLanguageSelect().ToString();
            String expected = String.Format(
                "<span class=\"fa fa-globe\"></span>" +
                "<div class=\"language-container dropdown-toggle\" data-toggle=\"dropdown\">" +
                    "<span class=\"current-language\">" +
                        "<img alt=\"\" src=\"{0}\" />" +
                        "English" +
                    "</span>" +
                    "<span class=\"caret\"></span>" +
                "</div>" +
                "<ul class=\"dropdown-menu\" role=\"menu\">" +
                    "<li>" +
                        "<a href=\"{2}?{4}\">" +
                        "<img src=\"{0}\" alt=\"\" />English</a>" +
                    "</li>" +
                    "<li>" +
                        "<a href=\"{3}?{4}\">" +
                        "<img src=\"{1}\" alt=\"\" />Lietuvių</a>" +
                    "</li>" +
                "</ul>",
                urlHelper.Content("~/Images/Flags/en.gif"),
                urlHelper.Content("~/Images/Flags/lt.gif"),
                urlHelper.Action(action, new { area = routeValues["area"], language = "en" }),
                urlHelper.Action(action, new { area = routeValues["area"], language = "lt" }),
                html.ViewContext.HttpContext.Request.QueryString);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
