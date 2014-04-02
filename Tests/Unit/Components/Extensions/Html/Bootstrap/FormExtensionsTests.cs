using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Template.Components.Extensions.Html;
using Template.Resources;
using Template.Tests.Helpers;
using Template.Tests.Objects.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class FormExtensionsTests
    {
        private Expression<Func<BootstrapModel, Object>> expression;
        private HtmlHelper<BootstrapModel> htmlHelper;
        private String validationClass;
        private String contentClass;
        private String labelClass;

        [SetUp]
        public void SetUp()
        {
            validationClass = Template.Components.Extensions.Html.FormExtensions.ValidationClass;
            contentClass = Template.Components.Extensions.Html.FormExtensions.ContentClass;
            labelClass = Template.Components.Extensions.Html.FormExtensions.LabelClass;
            htmlHelper = new HtmlMock<BootstrapModel>().Html;
            expression = null;
        }

        #region Extension method: ContentGroup(this HtmlHelper html)

        [Test]
        public void ContentGroup_WritesFormColumn()
        {
            var expected = new StringBuilder();
            new FormWrapper(new StringWriter(expected), contentClass).Dispose();

            var actualWriter = htmlHelper.ViewContext.Writer as StringWriter;
            var actual = actualWriter.GetStringBuilder();
            htmlHelper.ContentGroup().Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Extension method: FormGroup(this HtmlHelper html)

        [Test]
        public void FormGroup_WritesFormGroup()
        {
            var expected = new StringBuilder();
            new FormGroup(new StringWriter(expected)).Dispose();

            var actualWriter = htmlHelper.ViewContext.Writer as StringWriter;
            var actual = actualWriter.GetStringBuilder();
            htmlHelper.FormGroup().Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Extension method: InputGroup(this HtmlHelper html)

        [Test]
        public void InputGroup_WritesInputGroup()
        {
            var expected = new StringBuilder();
            new InputGroup(new StringWriter(expected)).Dispose();

            var actualWriter = htmlHelper.ViewContext.Writer as StringWriter;
            var actual = actualWriter.GetStringBuilder();
            htmlHelper.InputGroup().Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Extension method: FormActions(this HtmlHelper html)

        [Test]
        public void FormActions_WritesFormActions()
        {
            var expected = new StringBuilder();
            new FormActions(new StringWriter(expected)).Dispose();

            var actualWriter = htmlHelper.ViewContext.Writer as StringWriter;
            var actual = actualWriter.GetStringBuilder();
            htmlHelper.FormActions().Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Extension method: FormSubmit(this HtmlHelper html)

        [Test]
        public void FormSubmit_FormsSubmitHtml()
        {
            var expectedInput = new TagBuilder("input");
            expectedInput.AddCssClass("btn btn-primary");
            expectedInput.MergeAttribute("type", "submit");
            expectedInput.MergeAttribute("value", Template.Resources.Shared.Resources.Submit);

            var expected = expectedInput.ToString(TagRenderMode.SelfClosing);
            var actual = htmlHelper.FormSubmit().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: BootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapLabelFor_FormsLabelFor()
        {
            expression = (model) => model.Required;

            var expectedInput = new TagBuilder("label");
            expectedInput.AddCssClass(labelClass);
            expectedInput.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            expectedInput.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            var expected = expectedInput.ToString();
            var actual = htmlHelper.BootstrapLabelFor(expression).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: BootstrapRequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapRequiredLabelFor_FormsRequiredLabelFor()
        {
            expression = (model) => model.NotRequired;

            var expectedLabel = new TagBuilder("label");
            expectedLabel.AddCssClass(labelClass);
            expectedLabel.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            expectedLabel.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            var requiredSpan = new TagBuilder("span");
            requiredSpan.AddCssClass("required");
            requiredSpan.InnerHtml = " *";

            expectedLabel.InnerHtml += requiredSpan.ToString();

            var expected = expectedLabel.ToString();
            var actual = htmlHelper.BootstrapRequiredLabelFor(expression).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: BootstrapFormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapFormLabelFor_OnNonMemberExpressionThrows()
        {
            expression = (model) => model.Method();
            Assert.Throws<InvalidOperationException>(() => htmlHelper.BootstrapFormLabelFor(model => model.Method()), "Expression must be a member expression");
        }

        [Test]
        public void BootstrapFormLabelFor_FormsLabelFor()
        {
            expression = (model) => model.NotRequired;

            var expectedLabel = new TagBuilder("label");
            expectedLabel.AddCssClass(labelClass);
            expectedLabel.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            expectedLabel.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            var expected = expectedLabel.ToString();
            var actual = htmlHelper.BootstrapFormLabelFor(expression).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BootstrapFormLabelFor_FormsRequiredLabelFor()
        {
            expression = (model) => model.Required;

            var expectedLabel = new TagBuilder("label");
            expectedLabel.AddCssClass(labelClass);
            expectedLabel.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            expectedLabel.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            var requiredSpan = new TagBuilder("span");
            requiredSpan.AddCssClass("required");
            requiredSpan.InnerHtml = " *";

            expectedLabel.InnerHtml += requiredSpan.ToString();

            var expected = expectedLabel.ToString();
            var actual = htmlHelper.BootstrapFormLabelFor(expression).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: BootstrapTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format = null, Object htmlAttributes = null)

        [Test]
        public void BootstrapTextBoxFor_FormsTextBoxFor()
        {
            expression = (model) => model.NotRequired;
            var attributes = new { @class = "form-control", autocomplete = "off" };
            var formColumn = new FormWrapper(htmlHelper.TextBoxFor(expression, null, attributes), contentClass);

            var expected = formColumn.ToString();
            var actual = htmlHelper.BootstrapTextBoxFor(expression).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BootstrapTextBoxFor_UsesFormat()
        {
            Expression<Func<BootstrapModel, Decimal>> expression = (model) => model.Number;
            var attributes = new { @class = "form-control", autocomplete = "off" };
            var formColumn = new FormWrapper(htmlHelper.TextBoxFor(expression, "{0:0.00}", attributes), contentClass);

            var expected = formColumn.ToString();
            var actual = htmlHelper.BootstrapTextBoxFor(expression, "{0:0.00}").ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BootstrapTextBoxFor_AddsAttributes()
        {
            expression = (model) => model.NotRequired;
            var attributes = new { @class = "test form-control", autocomplete = "off" };
            var formColumn = new FormWrapper(htmlHelper.TextBoxFor(expression, null, attributes), contentClass);

            var expected = formColumn.ToString();
            var actual = htmlHelper.BootstrapTextBoxFor(expression, null, new { @class = " test" }).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BootstrapTextBoxFor_DoesNotOverwriteAutocompleteAttribute()
        {
            expression = (model) => model.NotRequired;
            var attributes = new { @class = "form-control", autocomplete = "on" };
            var formColumn = new FormWrapper(htmlHelper.TextBoxFor(expression, null, attributes), contentClass);

            var expected = formColumn.ToString();
            var actual = htmlHelper.BootstrapTextBoxFor(expression, null, new { autocomplete = "on" }).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: BootstrapDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapDatePickerFor_FormsDatePicker()
        {
            Expression<Func<BootstrapModel, DateTime>> expression = (model) => model.Date;
            var format = String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

            var expected = htmlHelper.BootstrapTextBoxFor(expression, format, new { @class = "datepicker" }).ToString();
            var actual = htmlHelper.BootstrapDatePickerFor(expression).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: BootstrapPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapPasswordFor_FormsPasswordFor()
        {
            expression = (model) => model.Required;
            var formColumn = new FormWrapper(htmlHelper.PasswordFor(expression, new { @class = "form-control" }), contentClass);

            var expected = formColumn.ToString();
            var actual = htmlHelper.BootstrapPasswordFor(expression).ToString();
            
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: BootstrapValidationFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Test]
        public void BootstrapValidationFor_FormsValidationFor()
        {
            expression = (model) => model.Required;
            var formColumn = new FormWrapper(htmlHelper.ValidationMessageFor(expression), validationClass);

            var expected = formColumn.ToString();
            var actual = htmlHelper.BootstrapValidationFor(expression).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
