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
            StringWriter writer = new StringWriter(new StringBuilder());
            using (new FormWrapper(writer, "test-class")) writer.Write("Content");

            String expected = "<div class=\"test-class\">Content</div>";
            String actual = writer.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_ClosesWrapperDiv()
        {
            StringWriter writer = new StringWriter(new StringBuilder());
            using (new FormWrapper(writer, "test-class")) writer.Write("Content");

            String expected = "<div class=\"test-class\">Content</div>";
            String actual = writer.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            StringWriter writer = new StringWriter(new StringBuilder());
            FormWrapper formActions = new FormWrapper(writer, "test-class");
            writer.Write("Content");
            formActions.Dispose();
            formActions.Dispose();

            String expected = "<div class=\"test-class\">Content</div>";
            String actual = writer.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
