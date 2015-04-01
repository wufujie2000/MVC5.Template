using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Mvc;
using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class BootstrapExtensionsTests : IDisposable
    {
        private HtmlHelper<BootstrapModel> html;
        private BootstrapModel model;

        public BootstrapExtensionsTests()
        {
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
            html = HtmlHelperFactory.CreateHtmlHelper(new BootstrapModel());
            model = html.ViewData.Model;
        }
        public void Dispose()
        {
            GlobalizationManager.Provider = null;
        }

        #region Extension method: FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormLabelFor_OnNotMemberExpressionThrows()
        {
            Exception exception = Assert.Throws<InvalidOperationException>(() => html.FormLabelFor(expression => expression.GetType()));

            String expected = "Expression must be a member expression.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsRequiredLabel()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.Required;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label for=\"{0}\">" +
                    "<span class=\"required\"> *</span>" +
                "</label>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsRequiredLabelOnValueTypes()
        {
            Expression<Func<BootstrapModel, Int64>> expression = (exp) => exp.Relation.RequiredValue;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label for=\"{0}\">" +
                    "<span class=\"required\"> *</span>" +
                "</label>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsNotRequiredLabel()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label for=\"{0}\"></label>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsNotRequiredLabelOnNullableValueTypes()
        {
            Expression<Func<BootstrapModel, Int64?>> expression = (exp) => exp.Relation.NotRequiredNullableValue;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label for=\"{0}\"></label>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormTextBoxFor_FormsNotAutocompletableTextBox()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Editable;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Editable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableTrue;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"readonly\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format)

        [Fact]
        public void FormTextBoxFor_Format_FormsNotAutocompletableTextBox()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Editable;

            String actual = html.FormTextBoxFor(expression, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Editable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableTrue;

            String actual = html.FormTextBoxFor(expression, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"readonly\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_FormatsTextBoxValue()
        {
            Expression<Func<BootstrapModel, Decimal>> expression = (exp) => exp.Relation.Number;

            String actual = html.FormTextBoxFor(expression, "{0:0.00}").ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormTextBoxFor_Attributes_MergesClassAttributes()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_FormsNotAutocompletableTextBox()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotOverwriteAutocompleteAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"on\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"false\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Editable;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Editable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableTrue;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"readonly\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format, Object htmlAttributes)

        [Fact]
        public void FormTextBoxFor_Format_Attributes_MergesClassAttributes()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_FormsNotAutocompletableTextBox()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotOverwriteAutocompleteAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null, new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"on\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, null, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"false\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Editable;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Editable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableTrue;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"readonly\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_FormatsTextBoxValue()
        {
            Expression<Func<BootstrapModel, Decimal>> expression = (exp) => exp.Relation.Number;

            String actual = html.FormTextBoxFor(expression, "{0:0.00}", null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Fact]
        public void FormPasswordFor_FormsNotAutocompletablePasswordInput()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.Required;

            String actual = html.FormPasswordFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"password\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormTextAreaFor_FormsNotAutocompletableTextArea()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextAreaFor(expression).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"{0}\" name=\"{1}\" rows=\"6\">{2}</textarea>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                Environment.NewLine + model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Editable;

            String actual = html.FormTextAreaFor(expression).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"{0}\" name=\"{1}\" rows=\"6\">{2}</textarea>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                Environment.NewLine + model.Editable);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableTrue;

            String actual = html.FormTextAreaFor(expression).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"{0}\" name=\"{1}\" rows=\"6\">{2}</textarea>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                Environment.NewLine + model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextAreaFor(expression).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"{0}\" name=\"{1}\" readonly=\"readonly\" rows=\"6\">{2}</textarea>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                Environment.NewLine + model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormDatePickerFor_FormsDatePicker()
        {
            Expression<Func<BootstrapModel, DateTime?>> expression = (exp) => exp.Relation.Date;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("lt-LT");

            String actual = html.FormDatePickerFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datepicker\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.Date.Value.ToString("yyyy.MM.dd"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormDatePickerFor_FormsDateTimePicker()
        {
            Expression<Func<BootstrapModel, DateTime?>> expression = (exp) => exp.Relation.Date;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("lt-LT");

            String actual = html.FormDateTimePickerFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datetimepicker\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.Date.Value.ToString("yyyy.MM.dd HH:mm"));

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
