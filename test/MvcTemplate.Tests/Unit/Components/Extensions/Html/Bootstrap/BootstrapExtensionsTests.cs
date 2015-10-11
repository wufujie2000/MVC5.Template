using MvcTemplate.Components.Extensions.Html;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class BootstrapExtensionsTests
    {
        private HtmlHelper<BootstrapModel> html;
        private BootstrapModel model;

        public BootstrapExtensionsTests()
        {
            html = HtmlHelperFactory.CreateHtmlHelper(new BootstrapModel());
            model = html.ViewData.Model;
        }

        #region Extension method: FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Boolean? required = null)

        [Fact]
        public void FormLabelFor_OnNotMemberExpressionThrows()
        {
            Exception exception = Assert.Throws<InvalidOperationException>(() => html.FormLabelFor(expression => expression.GetType(), required: true));

            String expected = "Expression must be a member expression.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsOverwrittenRequiredLabel()
        {
            String actual = html.FormLabelFor(x => x.Relation.NotRequired, required: true).ToString();
            String expected =
                "<label for=\"Relation_NotRequired\">" +
                    "<span class=\"require\">*</span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsOverwrittenNotRequiredLabel()
        {
            String actual = html.FormLabelFor(x => x.Relation.Required, required: false).ToString();
            String expected =
                "<label for=\"Relation_Required\">" +
                    "<span class=\"require\"></span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsRequiredLabel()
        {
            String actual = html.FormLabelFor(x => x.Relation.Required).ToString();
            String expected =
                "<label for=\"Relation_Required\">" +
                    "<span class=\"require\">*</span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsRequiredLabelOnValueTypes()
        {
            String actual = html.FormLabelFor(x => x.Relation.Number).ToString();
            String expected =
                "<label for=\"Relation_Number\">" +
                    "<span class=\"require\">*</span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsNotRequiredLabel()
        {
            String actual = html.FormLabelFor(x => x.Relation.NotRequired).ToString();
            String expected =
                "<label for=\"Relation_NotRequired\">" +
                    "<span class=\"require\"></span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_FormsNotRequiredLabelOnNullableValueTypes()
        {
            String actual = html.FormLabelFor(x => x.Relation.Date).ToString();
            String expected =
                "<label for=\"Relation_Date\">" +
                    "<span class=\"require\"></span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormTextBoxFor_DoesNotAddReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.Number).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Number\" name=\"Number\" type=\"text\" value=\"{0}\" />",
                model.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format)

        [Fact]
        public void FormTextBoxFor_Format_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue, "{0}").ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, "{0}").ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_FormatsTextBoxValue()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.Number, "{0:0.00}").ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_Number\" name=\"Relation.Number\" type=\"text\" value=\"{0}\" />",
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormTextBoxFor_Attributes_MergesClassAttributes()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotOverwriteAutocompleteAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"on\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"false\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue, new { data_tab = "Test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" data-tab=\"Test\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Attributes_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, new { data_tab = "Test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" data-tab=\"Test\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format, Object htmlAttributes)

        [Fact]
        public void FormTextBoxFor_Format_Attributes_MergesClassAttributes()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.Number, "{0:0.00}", new { @class = "test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"Relation_Number\" name=\"Relation.Number\" type=\"text\" value=\"{0}\" />",
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotOverwriteAutocompleteAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, "{0}", new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"on\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, "{0}", new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"false\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue, "{0}", new { data_tab = "Test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" data-tab=\"Test\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_Format_Attributes_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, "{0}", new { data_tab = "Test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" data-tab=\"Test\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Fact]
        public void FormPasswordFor_FormsNotAutocompletablePasswordInput()
        {
            String expected = "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_Required\" name=\"Relation.Required\" type=\"password\" />";
            String actual = html.FormPasswordFor(x => x.Relation.Required).ToString();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormTextAreaFor_DoesNotAddReadOnlyAttribute()
        {
            String actual = html.FormTextAreaFor(x => x.Number).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"Number\" name=\"Number\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextAreaFor(x => x.EditableTrue).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"EditableTrue\" name=\"EditableTrue\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextAreaFor(x => x.EditableFalse).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormTextAreaFor_Attributes_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            String actual = html.FormTextAreaFor(x => x.EditableTrue, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control test\" cols=\"20\" id=\"EditableTrue\" name=\"EditableTrue\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_Attributes_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            String actual = html.FormTextAreaFor(x => x.EditableFalse, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control test\" cols=\"20\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_Attributes_DoesNotOverrideSpecifiedRows()
        {
            String actual = html.FormTextAreaFor(x => x.NotRequired, new { rows = "12" }).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"NotRequired\" name=\"NotRequired\" rows=\"12\">{0}</textarea>",
                Environment.NewLine + model.NotRequired);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormDatePickerFor_FormsDatePicker()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");

            String actual = html.FormDatePickerFor(x => x.Relation.Date).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datepicker\" id=\"Relation_Date\" name=\"Relation.Date\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy/MM/dd"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormDatePickerFor_FormsDatePickerWtihAttributes()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");

            String actual = html.FormDatePickerFor(x => x.Relation.Date, new { @readonly = "readonly" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datepicker\" id=\"Relation_Date\" name=\"Relation.Date\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy/MM/dd"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormDatePickerFor_FormsDateTimePicker()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");

            String actual = html.FormDateTimePickerFor(x => x.Relation.Date).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datetimepicker\" id=\"Relation_Date\" name=\"Relation.Date\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy/MM/dd H:mm"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormDatePickerFor_FormsDateTimePickerWithAttributes()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");

            String actual = html.FormDateTimePickerFor(x => x.Relation.Date, new { @readonly = "readonly" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datetimepicker\" id=\"Relation_Date\" name=\"Relation.Date\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy/MM/dd H:mm"));

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
