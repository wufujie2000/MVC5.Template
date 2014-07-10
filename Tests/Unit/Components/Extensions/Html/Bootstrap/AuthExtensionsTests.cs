using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

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
            html = new HtmlMock<BootstrapModel>(model).Html;
            expression = (expModel) => expModel.Relation.Required;
        }

        #region Extension method: AuthUsernameFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)

        [Test]
        public void AuthUsernameFor_FormsUsernameGroupElements()
        {
            String actual = html.AuthUsernameFor(expression).ToString();
            String expected = String.Format(
                "<span class=\"input-group-addon\">" +
                    "<i class=\"fa fa-user\"></i>" +
                "</span>" +
                "<input class=\"form-control\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.Required);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthPasswordFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)

        [Test]
        public void AuthPasswordFor_FormsPasswordGroupElements()
        {
            String actual = html.AuthPasswordFor(expression).ToString();
            String expected = String.Format(
                "<span class=\"input-group-addon\">" +
                    "<i class=\"fa fa-lock\"></i>" +
                "</span>" +
                "<input class=\"form-control\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"password\" />",
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
                "<span class=\"input-group-addon\">" +
                    "<i class=\"fa fa-envelope\"></i>" +
                "</span>" +
                "<input class=\"form-control\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.Required);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthLanguageSelect<TModel>(this HtmlHelper<TModel> html)

        [Test]
        public void AuthLanguageSelect_FormsLanguageSelectGroupElements()
        {
            String actual = html.AuthLanguageSelect().ToString();
            String expected =
                "<span class=\"input-group-addon flag-span\">" +
                "<i class=\"fa fa-flag\"></i>" +
                "</span>" +
                "<input class=\"form-control\" id=\"TempLanguage\" type=\"text\"></input>" +
                "<select id=\"Language\">" +
                    "<option value=\"en-GB\">English</option>" +
                    "<option value=\"lt-LT\">Lietuvių</option>" +
                "</select>";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
