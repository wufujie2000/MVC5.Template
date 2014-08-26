using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class BootstrapExtensionsTests
    {
        private HtmlHelper<BootstrapModel> html;
        private StringBuilder htmlStringBuilder;
        private BootstrapModel model;

        [SetUp]
        public void SetUp()
        {
            model = new BootstrapModel();
            html = HtmlHelperFactory.CreateHtmlHelper(model);
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
            htmlStringBuilder = (html.ViewContext.Writer as StringWriter).GetStringBuilder();
        }

        #region Extension method: FormGroup(this HtmlHelper html)

        [Test]
        public void FormGroup_WritesFormGroup()
        {
            html.FormGroup().Dispose();

            String expected = "<div class=\"form-group\"></div>";
            String actual = htmlStringBuilder.ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: InputGroup(this HtmlHelper html)

        [Test]
        public void InputGroup_WritesInputGroup()
        {
            html.InputGroup().Dispose();

            String expected = "<div class=\"input-group\"></div>";
            String actual = htmlStringBuilder.ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormActions(this HtmlHelper html)

        [Test]
        public void FormActions_WritesFormActions()
        {
            html.FormActions().Dispose();

            String expected = String.Format("<div class=\"{0}\"></div>", BootstrapExtensions.FormActionsClass);
            String actual = htmlStringBuilder.ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: ContentGroup(this HtmlHelper html)

        [Test]
        public void ContentGroup_WritesContentGroup()
        {
            html.ContentGroup().Dispose();

            String expected = String.Format("<div class=\"{0}\"></div>", BootstrapExtensions.ContentClass);
            String actual = htmlStringBuilder.ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: Submit(this HtmlHelper html)

        [Test]
        public void FormSubmit_FormsSubmit()
        {
            String actual = html.Submit().ToString();
            String expected = String.Format(
                "<input class=\"btn btn-primary\" type=\"submit\" value=\"{0}\" />",
                MvcTemplate.Resources.Shared.Resources.Submit);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: SubmitFor(this HtmlHelper html, String value)

        [Test]
        public void Submit_FormsSubmitForValue()
        {
            String expected = "<input class=\"btn btn-primary\" type=\"submit\" value=\"Value\" />";
            String actual = html.SubmitFor("Value").ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormLabelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Test]
        public void FormLabelFor_OnNotMemberExpressionThrows()
        {
            Assert.Throws<InvalidOperationException>(() => html.FormLabelFor(expression => expression.ToString()));
        }

        [Test]
        public void FormLabelFor_FormsRequiredLabel()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.Required;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label class=\"{0}\" for=\"{1}\">" +
                    "<span class=\"required\"> *</span>" +
                "</label>",
                BootstrapExtensions.LabelClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormLabelFor_FormsNotRequiredLabel()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label class=\"{0}\" for=\"{1}\"></label>",
                BootstrapExtensions.LabelClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void FormTextBoxFor_FormsNotAutocompletableTextBox()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_OnNotMemberExpressionThrows()
        {
            Assert.Throws<InvalidOperationException>(() => html.FormTextBoxFor(expression => expression.ToString()));
        }

        [Test]
        public void FormTextBoxFor_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.NotReadOnly;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.NotReadOnly);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_DoesNotAddFalseReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyFalse;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyFalse);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_AddsReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyTrue;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" readonly=\"readonly\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyTrue);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format)

        [Test]
        public void FormTextBoxFor_Format_FormsNotAutocompletableTextBox()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, (String)null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_OnNotMemberExpressionThrows()
        {
            Assert.Throws<InvalidOperationException>(() => html.FormTextBoxFor(expression => expression.ToString(), (String)null));
        }

        [Test]
        public void FormTextBoxFor_Format_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.NotReadOnly;

            String actual = html.FormTextBoxFor(expression, (String)null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.NotReadOnly);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_DoesNotAddFalseReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyFalse;

            String actual = html.FormTextBoxFor(expression, (String)null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyFalse);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_AddsReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyTrue;

            String actual = html.FormTextBoxFor(expression, (String)null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" readonly=\"readonly\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyTrue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_UsesFormat()
        {
            Expression<Func<BootstrapModel, Decimal>> expression = (exp) => exp.Relation.Number;

            String actual = html.FormTextBoxFor(expression, "{0:0.00}").ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Test]
        public void FormTextBoxFor_Attributes_MergesClassAttributes()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control test\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_FormsNotAutocompletableTextBox()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_DoesNotOverwriteAutocompleteAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"on\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyTrue;

            String actual = html.FormTextBoxFor(expression, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" readonly=\"false\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyTrue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_OnNotMemberExpressionThrows()
        {
            Assert.Throws<InvalidOperationException>(() => html.FormTextBoxFor(expression => expression.ToString(), (Object)null));
        }

        [Test]
        public void FormTextBoxFor_Attributes_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.NotReadOnly;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.NotReadOnly);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_DoesNotAddFalseReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyFalse;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyFalse);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_AddsReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyTrue;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" readonly=\"readonly\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyTrue);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format, Object htmlAttributes)

        [Test]
        public void FormTextBoxFor_Format_Attributes_MergesClassAttributes()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control test\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_FormsNotAutocompletableTextBox()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_DoesNotOverwriteAutocompleteAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null, new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"on\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyTrue;

            String actual = html.FormTextBoxFor(expression, null, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" readonly=\"false\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyTrue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_OnNotMemberExpressionThrows()
        {
            Assert.Throws<InvalidOperationException>(() => html.FormTextBoxFor(expression => expression.ToString(), null, null));
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.NotReadOnly;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.NotReadOnly);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_DoesNotAddFalseReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyFalse;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyFalse);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_AddsReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.ReadOnlyTrue;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" readonly=\"readonly\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.ReadOnlyTrue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_UsesFormat()
        {
            Expression<Func<BootstrapModel, Decimal>> expression = (exp) => exp.Relation.Number;

            String actual = html.FormTextBoxFor(expression, "{0:0.00}", null).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void FormDatePickerFor_FormsDatePicker()
        {
            Expression<Func<BootstrapModel, DateTime>> expression = (exp) => exp.Relation.Date;

            String actual = html.FormDatePickerFor(expression).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control datepicker\" id=\"{1}\" name=\"{2}\" type=\"text\" value=\"{3}\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                String.Format(String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern),
                model.Relation.Date));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Test]
        public void FormPasswordFor_FormsNotAutocompletablePasswordInput()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.Required;

            String actual = actual = html.FormPasswordFor(expression).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<input autocomplete=\"off\" class=\"form-control\" id=\"{1}\" name=\"{2}\" type=\"password\" />" +
                "</div>",
                BootstrapExtensions.ContentClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormValidationFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Test]
        public void FormValidationFor_FormsValidation()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.Required;

            String actual = actual = html.FormValidationFor(expression).ToString();
            String expected = String.Format(
                "<div class=\"{0}\">" +
                    "<span class=\"field-validation-valid\" id=\"{1}_validationMessage\"></span>" +
                "</div>",
                BootstrapExtensions.ValidationClass,
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
