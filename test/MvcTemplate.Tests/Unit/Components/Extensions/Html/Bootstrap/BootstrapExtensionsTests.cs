using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class BootstrapExtensionsTests
    {
        private HtmlHelper<BootstrapModel> html;
        private StringBuilder htmlStringBuilder;
        private BootstrapModel model;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            model = new BootstrapModel();
            html = HtmlHelperFactory.CreateHtmlHelper(model);
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
        }

        [SetUp]
        public void SetUp()
        {
            html.ViewContext.Writer = new StringWriter();
            htmlStringBuilder = (html.ViewContext.Writer as StringWriter).GetStringBuilder();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            GlobalizationManager.Provider = null;
        }

        #region Extension method: FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void FormLabelFor_OnNotMemberExpressionThrows()
        {
            Exception expected = Assert.Throws<InvalidOperationException>(() => html.FormLabelFor(expression => expression.GetType()));
            Assert.AreEqual(expected.Message, "Expression must be a member expression.");
        }

        [Test]
        public void FormLabelFor_FormsRequiredLabel()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.Required;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label for=\"{0}\">" +
                    "<span class=\"required\"> *</span>" +
                "</label>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormLabelFor_FormsRequiredLabelOnValueTypes()
        {
            Expression<Func<BootstrapModel, Int64>> expression = (exp) => exp.Relation.RequiredValue;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label for=\"{0}\">" +
                    "<span class=\"required\"> *</span>" +
                "</label>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormLabelFor_FormsNotRequiredLabel()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label for=\"{0}\"></label>",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormLabelFor_FormsNotRequiredLabelOnNullableValueTypes()
        {
            Expression<Func<BootstrapModel, Int64?>> expression = (exp) => exp.Relation.NotRequiredNullableValue;

            String actual = html.FormLabelFor(expression).ToString();
            String expected = String.Format(
                "<label for=\"{0}\"></label>",
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
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_OnNotMemberExpressionThrows()
        {
            Exception expected = Assert.Throws<InvalidOperationException>(() => html.FormTextBoxFor(expression => expression.GetType()));
            Assert.AreEqual(expected.Message, "Expression must be a member expression.");
        }

        [Test]
        public void FormTextBoxFor_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Editable;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Editable);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableTrue;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableTrue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"readonly\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format)

        [Test]
        public void FormTextBoxFor_Format_FormsNotAutocompletableTextBox()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.NotRequired;

            String actual = html.FormTextBoxFor(expression, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_OnNotMemberExpressionThrows()
        {
            Exception expected = Assert.Throws<InvalidOperationException>(() => html.FormTextBoxFor(expression => expression.GetType(), null));
            Assert.AreEqual(expected.Message, "Expression must be a member expression.");
        }

        [Test]
        public void FormTextBoxFor_Format_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Editable;

            String actual = html.FormTextBoxFor(expression, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Editable);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableTrue;

            String actual = html.FormTextBoxFor(expression, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableTrue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"readonly\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_FormatsTextBoxValue()
        {
            Expression<Func<BootstrapModel, Decimal>> expression = (exp) => exp.Relation.Number;

            String actual = html.FormTextBoxFor(expression, "{0:0.00}").ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
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
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
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
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
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
                "<input autocomplete=\"on\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"false\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_OnNotMemberExpressionThrows()
        {
            Exception expected = Assert.Throws<InvalidOperationException>(() => html.FormTextBoxFor(expression => expression.GetType(), (Object)null));
            Assert.AreEqual(expected.Message, "Expression must be a member expression.");
        }

        [Test]
        public void FormTextBoxFor_Attributes_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Editable;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Editable);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableTrue;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableTrue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Attributes_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, (Object)null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"readonly\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

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
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
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
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
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
                "<input autocomplete=\"on\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Relation.NotRequired);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_DoesNotOverwriteReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, null, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"false\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_OnNotMemberExpressionThrows()
        {
            Exception expected = Assert.Throws<InvalidOperationException>(() => html.FormTextBoxFor(expression => expression.GetType(), null, null));
            Assert.AreEqual(expected.Message, "Expression must be a member expression.");
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_DoesNotAddReadOnlyAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Editable;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.Editable);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_DoesNotAddReadOnlyAttributeOnEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableTrue;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableTrue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_AddsReadOnlyAttributeOnNotEditableProperty()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.EditableFalse;

            String actual = html.FormTextBoxFor(expression, null, null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" readonly=\"readonly\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                model.EditableFalse);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormTextBoxFor_Format_Attributes_FormatsTextBoxValue()
        {
            Expression<Func<BootstrapModel, Decimal>> expression = (exp) => exp.Relation.Number;

            String actual = html.FormTextBoxFor(expression, "{0:0.00}", null).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
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
            Expression<Func<BootstrapModel, DateTime?>> expression = (exp) => exp.Relation.Date;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("lt-LT");

            String actual = html.FormDatePickerFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datepicker\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                String.Format(String.Format("{{0:{0}}}", CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern),
                model.Relation.Date));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void FormDatePickerFor_FormsDateTimePicker()
        {
            Expression<Func<BootstrapModel, DateTime?>> expression = (exp) => exp.Relation.Date;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("lt-LT");

            String actual = html.FormDateTimePickerFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datetimepicker\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression),
                String.Format(String.Format(
                    "{{0:{0} {1}}}",
                    CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                    CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern),
                model.Relation.Date));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: FormPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Test]
        public void FormPasswordFor_FormsNotAutocompletablePasswordInput()
        {
            Expression<Func<BootstrapModel, String>> expression = (exp) => exp.Relation.Required;

            String actual = html.FormPasswordFor(expression).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"{0}\" name=\"{1}\" type=\"password\" />",
                TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)),
                ExpressionHelper.GetExpressionText(expression));

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
