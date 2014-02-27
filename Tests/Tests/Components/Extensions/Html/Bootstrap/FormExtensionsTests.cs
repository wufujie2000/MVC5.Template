using Moq;
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

namespace Template.Tests.Tests.Components.Extensions.Html
{
    [TestFixture]
    public class FormExtensionsTests
    {
        private HtmlHelperMock<BootstrapModel> htmlHelperMock;
        private HtmlHelper<BootstrapModel> htmlHelper;
        private String labelClass;

        [SetUp]
        public void SetUp()
        {
            labelClass = Template.Components.Extensions.Html.FormExtensions.LabelClass;
            htmlHelperMock = new HtmlHelperMock<BootstrapModel>();
            htmlHelper = htmlHelperMock.HtmlHelper;
        }

        #region Extension method: FormColumn(this HtmlHelper html)

        [Test]
        public void FormColumn_WritesFormColumn()
        {
            var expected = new StringBuilder();
            var expectedWriter = new StringWriter(expected);
            new FormColumn(expectedWriter).Dispose();

            var actualWriter = htmlHelper.ViewContext.Writer as StringWriter;
            var actual = actualWriter.GetStringBuilder();
            htmlHelper.FormColumn().Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Extension method: FormGroup(this HtmlHelper html)

        [Test]
        public void FormGroup_WritesFormGroup()
        {
            var expected = new StringBuilder();
            var expectedWriter = new StringWriter(expected);
            new FormGroup(expectedWriter).Dispose();

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
            var expectedWriter = new StringWriter(expected);
            new InputGroup(expectedWriter).Dispose();

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
            var expectedWriter = new StringWriter(expected);
            new FormActions(expectedWriter).Dispose();

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
            var submit = new TagBuilder("input");
            submit.AddCssClass("btn btn-primary");
            submit.MergeAttribute("type", "submit");
            submit.MergeAttribute("value", Template.Resources.Shared.Resources.Submit);

            Assert.AreEqual(submit.ToString(TagRenderMode.SelfClosing), htmlHelper.FormSubmit().ToString());
        }

        #endregion

        #region Extension method: BootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapLabelFor_FormsLabelFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Required;

            var label = new TagBuilder("label");
            label.AddCssClass(labelClass);
            label.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            label.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(label.ToString(), htmlHelper.BootstrapLabelFor(expression).ToString());
        }

        #endregion

        #region Extension method: BootstrapRequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapRequiredLabelFor_FormsRequiredLabelFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.NotRequired;

            var requiredLabel = new TagBuilder("label");
            requiredLabel.AddCssClass(labelClass);
            requiredLabel.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            requiredLabel.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            var requiredSpan = new TagBuilder("span");
            requiredSpan.AddCssClass("required");
            requiredSpan.InnerHtml = " *";

            requiredLabel.InnerHtml += requiredSpan.ToString();

            Assert.AreEqual(requiredLabel.ToString(), htmlHelper.BootstrapRequiredLabelFor(expression).ToString());
        }

        #endregion

        #region Extension method: BootstrapFormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapFormLabelFor_OnNonMemberExpressionThrows()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Method();
            Assert.Throws<InvalidOperationException>(() => htmlHelper.BootstrapFormLabelFor(model => model.Method()), "Expression must be a member expression");
        }

        [Test]
        public void BootstrapFormLabelFor_FormsLabelFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.NotRequired;

            var label = new TagBuilder("label");
            label.AddCssClass(labelClass);
            label.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            label.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            Assert.AreEqual(label.ToString(), htmlHelper.BootstrapFormLabelFor(expression).ToString());
        }

        [Test]
        public void BootstrapFormLabelFor_FormsRequiredLabelFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Required;

            var requiredLabel = new TagBuilder("label");
            requiredLabel.AddCssClass(labelClass);
            requiredLabel.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            requiredLabel.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));

            var requiredSpan = new TagBuilder("span");
            requiredSpan.AddCssClass("required");
            requiredSpan.InnerHtml = " *";

            requiredLabel.InnerHtml += requiredSpan.ToString();

            Assert.AreEqual(requiredLabel.ToString(), htmlHelper.BootstrapFormLabelFor(expression).ToString());
        }

        #endregion

        #region Extension method: BootstrapTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format = null, Object htmlAttributes = null)

        [Test]
        public void BootstrapTextBoxFor_FormsTextBoxFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.NotRequired;

            var attributes = new { @class = "form-control", autocomplete = "off" };
            var formColumn = new FormColumn(htmlHelper.TextBoxFor(expression, null, attributes));

            Assert.AreEqual(formColumn.ToString(), htmlHelper.BootstrapTextBoxFor(expression).ToString());
        }

        [Test]
        public void BootstrapTextBoxFor_UsesFormat()
        {
            Expression<Func<BootstrapModel, Decimal>> expression = (model) => model.Number;

            var attributes = new { @class = "form-control", autocomplete = "off" };
            var formColumn = new FormColumn(htmlHelper.TextBoxFor(expression, "{0:0.00}", attributes));

            Assert.AreEqual(formColumn.ToString(), htmlHelper.BootstrapTextBoxFor(expression, "{0:0.00}").ToString());
        }

        [Test]
        public void BootstrapTextBoxFor_AddsAttributes()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.NotRequired;

            var attributes = new { @class = "test form-control", autocomplete = "off" };
            var formColumn = new FormColumn(htmlHelper.TextBoxFor(expression, null, attributes));

            Assert.AreEqual(formColumn.ToString(), htmlHelper.BootstrapTextBoxFor(expression, null, new { @class = " test" }).ToString());
        }

        [Test]
        public void BootstrapTextBoxFor_DoesNotOverwriteAutocompleteAttribute()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.NotRequired;

            var attributes = new { @class = "form-control", autocomplete = "on" };
            var formColumn = new FormColumn(htmlHelper.TextBoxFor(expression, null, attributes));

            Assert.AreEqual(formColumn.ToString(), htmlHelper.BootstrapTextBoxFor(expression, null, new { autocomplete = "on" }).ToString());
        }

        #endregion

        #region Extension method: BootstrapDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapDatePickerFor_FormsDatePicker()
        {
            Expression<Func<BootstrapModel, DateTime>> expression = (model) => model.Date;
            String format = String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

            Assert.AreEqual(
                htmlHelper.BootstrapTextBoxFor(expression, format, new { @class = "datepicker" }).ToString(),
                htmlHelper.BootstrapDatePickerFor(expression).ToString());
        }

        #endregion

        #region Extension method: BootstrapPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Test]
        public void BootstrapPasswordFor_FormsPasswordFor()
        {
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Required;
            var formColumn = new FormColumn(htmlHelper.PasswordFor(expression, new { @class = "form-control" }));

            Assert.AreEqual(formColumn.ToString(), htmlHelper.BootstrapPasswordFor(expression).ToString());
        }

        #endregion

        #region Extension method: BootstrapValidationFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Test]
        public void BootstrapValidationFor_FormsValidationFor()
        {
            htmlHelperMock.ViewContextMock.Object.ClientValidationEnabled = true;
            Expression<Func<BootstrapModel, String>> expression = (model) => model.Required;
            var formColumn = new FormColumn(htmlHelper.ValidationMessageFor(expression));

            Assert.AreEqual(formColumn.ToString(), htmlHelper.BootstrapValidationFor(expression).ToString());
        }

        #endregion
    }
}
