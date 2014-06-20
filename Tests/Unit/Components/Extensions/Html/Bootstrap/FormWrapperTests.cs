using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using Template.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class FormWrapperTests
    {
        #region Constructor: FormWrapper(String cssClass)

        [Test]
        public void FormWrapper_FormsWrapper()
        {
            String actual = new FormWrapper("test-class").ToString();
            String expected = "<div class=\"test-class\"></div>";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: FormWrapper(Object content, String cssClass)

        [Test]
        public void FormWrapper_FormsWrapperWithContent()
        {
            String actual = new FormWrapper("Content", "test-class").ToString();
            String expected = "<div class=\"test-class\">Content</div>";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: FormWrapper(ViewContext viewContext, String cssClass)

        [Test]
        public void FormWrapper_WritesWrapper()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            using (new FormWrapper(textWriter, "test-class")) textWriter.Write("Content");

            String actual = textWriter.GetStringBuilder().ToString();
            String expected = "<div class=\"test-class\">Content</div>";

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

            String actual = textWriter.GetStringBuilder().ToString();
            String expected = "<div class=\"test-class\">Content</div>";

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

            String actual = textWriter.GetStringBuilder().ToString();
            String expected = "<div class=\"test-class\">Content</div>";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
