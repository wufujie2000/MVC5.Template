using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

        [SetUp]
        public void SetUp()
        {
            model = new BootstrapModel();
            html = HtmlHelperFactory.CreateHtmlHelper(model);
            expression = (expModel) => expModel.Relation.Required;
            LocalizationManager.Provider = new LanguageProviderMock().Provider;
        }

        [TearDown]
        public void TearDown()
        {
            LocalizationManager.Provider = null;
        }

        #region Extension method: AuthUsernameFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression, Boolean autocomplete = true)

        [Test]
        public void AuthUsernameFor_FormsUsernameGroupElements()
        {
            String actual = html.AuthUsernameFor(expression).ToString();
            String expected = String.Format(
                "<span class=\"fa fa-user\"></span>" +
                "<input autocomplete=\"on\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.Required);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuthUsernameFor_FormsUsernameGroupElementsWithAutocompleteOff()
        {
            String actual = html.AuthUsernameFor(expression, autocomplete: false).ToString();
            String expected = String.Format(
                "<span class=\"fa fa-user\"></span>" +
                "<input autocomplete=\"off\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.Required);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthPasswordFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression, Boolean autocomplete = true)

        [Test]
        public void AuthPasswordFor_FormsPasswordGroupElements()
        {
            String actual = html.AuthPasswordFor(expression).ToString();
            String expected = String.Format(
                "<span class=\"fa fa-lock\"></span>" +
                "<input autocomplete=\"on\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"password\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuthPasswordFor_FormsPasswordGroupElementsWithAutocompleteOff()
        {
            String actual = html.AuthPasswordFor(expression, autocomplete: false).ToString();
            String expected = String.Format(
                "<span class=\"fa fa-lock\"></span>" +
                "<input autocomplete=\"off\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"password\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthEmailFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)

        [Test]
        public void AuthEmailFor_FormsEmailGroupElements()
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

        #region Extension method: AuthLanguageSelect<TModel>(this HtmlHelper<TModel> html)

        [Test]
        public void AuthLanguageSelect_OnSingleLanguageReturnsEmptyHtml()
        {
            List<Language> languages = new List<Language>() { new Language() };
            LocalizationManager.Provider = new LanguageProviderMock().Provider;
            LocalizationManager.Provider.Languages.Returns(languages);

            String actual = html.AuthLanguageSelect().ToString();
            String expected = String.Empty;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuthLanguageSelect_FormsLanguageSelectGroupElements()
        {
            RouteValueDictionary routeValues = html.ViewContext.RequestContext.RouteData.Values;
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            String action = routeValues["action"].ToString();

            String actual = html.AuthLanguageSelect().ToString();
            String expected = String.Format(
                "<span class=\"fa fa-flag\"></span>" +
                "<div class=\"language-container dropdown-toggle\" data-toggle=\"dropdown\">" +
                    "<span class=\"current-language\">" +
                        "<img alt=\"\" src=\"{0}\" />" +
                        "English" +
                    "</span>" +
                    "<span class=\"caret\"></span>" +
                "</div>" +
                "<ul class=\"dropdown-menu\" role=\"menu\">" +
                    "<li>" +
                        "<a href=\"{2}\">" +
                        "<img src=\"{0}\" alt=\"\" />English</a>" +
                    "</li>" +
                    "<li>" +
                        "<a href=\"{3}\">" +
                        "<img src=\"{1}\" alt=\"\" />Lietuvių</a>" +
                    "</li>" +
                "</ul>",
                urlHelper.Content("~/Images/Flags/en.gif"),
                urlHelper.Content("~/Images/Flags/lt.gif"),
                urlHelper.Action(action, new { language = "en", Param1 = "Value1" }),
                urlHelper.Action(action, new { language = "lt", Param1 = "Value1" }));

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
