using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class BootstrapExtensionsTests
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

        #region Extension method: ContentGroup(this HtmlHelper html)

        [Test]
        public void ContentGroup_WritesFormColumn()
        {
            html.ContentGroup().Dispose();

            String expected = String.Format("<div class=\"{0}\"></div>", BootstrapExtensions.ContentClass);
            String actual = (html.ViewContext.Writer as StringWriter).GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormGroup(this HtmlHelper html)

        [Test]
        public void FormGroup_WritesFormGroup()
        {
            html.FormGroup().Dispose();

            String expected = "<div class=\"form-group\"></div>";
            String actual = (html.ViewContext.Writer as StringWriter).GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: InputGroup(this HtmlHelper html)

        [Test]
        public void InputGroup_WritesInputGroup()
        {
            html.InputGroup().Dispose();

            String expected = "<div class=\"input-group\"></div>";
            String actual = (html.ViewContext.Writer as StringWriter).GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormActions(this HtmlHelper html)

        [Test]
        public void FormActions_WritesFormActions()
        {
            html.FormActions().Dispose();

            String expected = String.Format("<div class=\"{0}\"></div>", BootstrapExtensions.FormActionsClass);
            String actual = (html.ViewContext.Writer as StringWriter).GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: Submit(this HtmlHelper html, String value)

        [Test]
        public void Submit_FormsSubmitHtml()
        {
            String expected = "<input class=\"btn btn-primary\" type=\"submit\" value=\"Value\" />";
            String actual = html.Submit("Value").ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormSubmit(this HtmlHelper html)

        [Test]
        public void FormSubmit_FormsFormSubmitHtml()
        {
            String expected = String.Format("<input class=\"btn btn-primary\" type=\"submit\" value=\"{0}\" />", MvcTemplate.Resources.Shared.Resources.Submit);
            String actual = html.FormSubmit().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void FormLabelFor_OnNonMemberExpressionThrows()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Method();

            Assert.Throws<InvalidOperationException>(() => html.FormLabelFor(model => model.Method()), "Expression must be a member expression");
        }

        [Test]
        public void FormLabelFor_FormsRequiredLabelFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.Required;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format("<label class=\"{0}\" for=\"{1}\"><span class=\"required\"> *</span></label>",
                BootstrapExtensions.LabelClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormLabelFor_FormsNotRequiredLabelFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.NotRequired;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format("<label class=\"{0}\" for=\"{1}\"></label>",
                BootstrapExtensions.LabelClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format = null, Object htmlAttributes = null)

        [Test]
        public void FormTextBoxFor_FormsTextBoxFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format("<div class=\"{0}\"><input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" /></div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                bootstrapModel.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_UsesFormat()
        {
            Expression<Func<BootstrapModel, Decimal>> expression = (model) => model.Relation.Number;

            String actual = html.FormTextBoxFor(expression, "{0:0.00}").ToString();
            String expected = String.Format("<div class=\"{0}\"><input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" /></div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                String.Format("{0:0.00}", bootstrapModel.Relation.Number));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_AddsAttributes()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null, new { @class = "test" }).ToString();
            String expected = String.Format("<div class=\"{0}\"><input autocomplete=\"off\" class=\"test form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" /></div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                bootstrapModel.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_DoesNotOverwriteAutocompleteAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null, new { autocomplete = "on" }).ToString();
            String expected = String.Format("<div class=\"{0}\"><input autocomplete=\"on\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" /></div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                bootstrapModel.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void FormDatePickerFor_FormsDatePicker()
        {
            Expression<Func<BootstrapModel, DateTime>> expression = (model) => model.Relation.Date;

            String actual = html.FormDatePickerFor(expression).ToString();
            String expected = String.Format("<div class=\"{0}\"><input autocomplete=\"off\" class=\"datepicker form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" /></div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                String.Format(String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern), bootstrapModel.Relation.Date));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: BootstrapPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapPasswordFor_FormsPasswordFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.Required;

            String actual = actual = html.FormPasswordFor(expression).ToString();
            String expected = String.Format("<div class=\"{0}\"><input class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"password\" /></div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormValidationFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Test]
        public void FormValidationFor_FormsValidationFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Relation.Required;

            String actual = actual = html.FormValidationFor(expression).ToString();
            String expected = String.Format("<div class=\"{0}\"><span class=\"field-validation-valid\" id=\"{1}_validationMessage\"></span></div>",
                BootstrapExtensions.ValidationClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
