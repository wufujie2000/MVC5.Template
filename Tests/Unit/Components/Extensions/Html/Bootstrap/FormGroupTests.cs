using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using Template.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class FormGroupTests
    {
        #region Constructor: FormGroup(TextWriter writer)

        [Test]
        public void FormGroup_WritesFormGroup()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            using (new FormGroup(textWriter)) textWriter.Write("Content");

            String expected = "<div class=\"form-group\">Content</div>";
            String actual = textWriter.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanBeDisposedMultipleTimes()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            FormGroup formActions = new FormGroup(textWriter);
            textWriter.Write("Content");
            formActions.Dispose();
            formActions.Dispose();

            String expected = "<div class=\"form-group\">Content</div>";
            String actual = textWriter.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
