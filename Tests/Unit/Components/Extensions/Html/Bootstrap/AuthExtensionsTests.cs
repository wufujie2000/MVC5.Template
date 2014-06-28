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

        #region Extension method: AuthUsernameFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)

        [Test]
        public void AuthUsernameFor_FormsAuthUsernameFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.Required;

            String actual = html.AuthUsernameFor(expression).ToString();
            String expected = String.Format("<span class=\"input-group-addon\"><i class=\"fa fa-user\"></i></span><input class=\"form-control\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                bootstrapModel.Relation.Required);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthPasswordFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)

        [Test]
        public void AuthPasswordFor_FormsAuthPasswordFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.Required;

            String actual = html.AuthPasswordFor(expression).ToString();
            String expected = String.Format("<span class=\"input-group-addon\"><i class=\"fa fa-lock\"></i></span><input class=\"form-control\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"password\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthEmailFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)

        [Test]
        public void AuthEmailFor_FormsAuthEmailFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.Required;

            String actual = html.AuthEmailFor(expression).ToString();
            String expected = String.Format("<span class=\"input-group-addon\"><i class=\"fa fa-envelope\"></i></span><input class=\"form-control\" id=\"{0}\" name=\"{1}\" placeholder=\"\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                bootstrapModel.Relation.Required);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AuthLanguageSelect<TModel>(this HtmlHelper<TModel> html)

        [Test]
        public void AuthLanguageSelect_FormsAuthLanguageSelect()
        {
            String actual = html.AuthLanguageSelect().ToString();
            String expected = String.Format("<span class=\"input-group-addon flag-span\"><i class=\"fa fa-flag\"></i></span><input class=\"form-control\" id=\"TempLanguage\" type=\"text\"></input><select id=\"Language\"><option value=\"{0}\">{1}</option><option value=\"{2}\">{3}</option></select>",
                "en-GB", "English",
                "lt-LT", "Lietuvių");

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
