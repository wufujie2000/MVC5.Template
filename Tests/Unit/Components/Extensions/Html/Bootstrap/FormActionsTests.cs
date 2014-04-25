using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using Template.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class FormActionsTests
    {
        #region Constructor: FormActions(TextWriter writer, String cssClass)

        [Test]
        public void FormActions_WritesFormActions()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            using (new FormActions(textWriter, "css-class")) textWriter.Write("Content");

            String expected = "<div class=\"form-group\"><div class=\"css-class\">Content</div></div>";
            String actual = textWriter.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanBeDisposedMultipleTimes()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            FormActions formActions = new FormActions(textWriter, "css-class");
            textWriter.Write("Content");
            formActions.Dispose();
            formActions.Dispose();

            String expected = "<div class=\"form-group\"><div class=\"css-class\">Content</div></div>";
            String actual = textWriter.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
