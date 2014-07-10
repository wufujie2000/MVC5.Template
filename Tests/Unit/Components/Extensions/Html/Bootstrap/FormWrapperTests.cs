using MvcTemplate.Components.Extensions.Html;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class FormWrapperTests
    {
        #region Constructor: FormWrapper(TextWriter writer, String cssClass)

        [Test]
        public void FormWrapper_WritesWrapper()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            using (new FormWrapper(textWriter, "test-class")) textWriter.Write("Content");

            String expected = "<div class=\"test-class\">Content</div>";
            String actual = textWriter.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_ClosesWrapperDiv()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            FormWrapper formActions = new FormWrapper(textWriter, "test-class");
            textWriter.Write("Content");
            formActions.Dispose();

            String expected = "<div class=\"test-class\">Content</div>";
            String actual = textWriter.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            FormWrapper formActions = new FormWrapper(textWriter, "test-class");
            textWriter.Write("Content");
            formActions.Dispose();
            formActions.Dispose();

            String expected = "<div class=\"test-class\">Content</div>";
            String actual = textWriter.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
