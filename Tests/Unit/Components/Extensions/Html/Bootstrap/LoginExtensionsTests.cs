using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Template.Components.Extensions.Html;
using Template.Tests.Helpers;
using Template.Tests.Objects;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class LoginExtensionsTests
    {
        private HtmlHelper<BootstrapModel> html;
        private BootstrapModel bootstrapModel;

        [SetUp]
        public void SetUp()
        {
            bootstrapModel = new BootstrapModel()
            {
                NotRequired = "NotRequired",
                Required = "Required",
                Date = DateTime.Now,
                Number = 10.7854M,
                Relation = new BootstrapModel()
                {
                    NotRequired = "NotRequiredRelation",
                    Date = new DateTime(2011, 01, 01),
                    Required = "RequiredRelation",
                    Number = 1.6666M,
                }
            };

            html = new HtmlMock<BootstrapModel>(bootstrapModel).Html;
        }

        #region Extensions method: LoginUsernameFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)

        [Test]
        public void LoginUsernameFor_FormsLoginUsernameFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.Required;

            String actual = html.LoginUsernameFor(expression).ToString();
            String expected = String.Format("<span class=\"input-group-addon\"><i class=\"fa fa-user\"></i></span><input class=\"form-control\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                bootstrapModel.Relation.Required);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extensions method: LoginPasswordFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)

        [Test]
        public void LoginPasswordFor_FormsLoginPasswordFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.Required;

            String actual = html.LoginPasswordFor(expression).ToString();
            String expected = String.Format("<span class=\"input-group-addon lock-span\"><i class=\"fa fa-lock\"></i></span><input class=\"form-control\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"password\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extensions method: LoginLanguageSelect<TModel>(this HtmlHelper<TModel> html)

        [Test]
        public void LoginLanguageSelect_FormsLoginLanguageSelect()
        {
            String actual = html.LoginLanguageSelect().ToString();
            String expected = String.Format("<span class=\"input-group-addon flag-span\"><i class=\"fa fa-flag\"></i></span><input class=\"form-control\" id=\"TempLanguage\" type=\"text\"></input><select id=\"Language\"><option value=\"{0}\">{1}</option><option value=\"{2}\">{3}</option></select>",
                "en-GB", "English",
                "lt-LT", "Lietuvių");

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extensions method: LoginSubmit<TModel>(this HtmlHelper<TModel> html)

        [Test]
        public void LoginSubmit_FormsLoginSubmit()
        {
            String actual = html.LoginSubmit().ToString();
            String expected = String.Format("<div class=\"login-form-actions\"><input class=\"btn btn-block btn-primary btn-default\" type=\"submit\" value=\"{0}\"></input></div>",
                Template.Resources.Shared.Resources.Login);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
