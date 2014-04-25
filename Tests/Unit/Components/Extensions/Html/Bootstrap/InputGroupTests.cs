using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using Template.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class InputGroupTests
    {
        #region Constructor: InputGroup(TextWriter writer)

        [Test]
        public void InputGroup_WritesInputGroup()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            using (new InputGroup(textWriter)) textWriter.Write("Content");

            String expected = "<div class=\"input-group\">Content</div>";
            String actual = textWriter.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanBeDisposedMultipleTimes()
        {
            StringWriter textWriter = new StringWriter(new StringBuilder());
            InputGroup formActions = new InputGroup(textWriter);
            textWriter.Write("Content");
            formActions.Dispose();
            formActions.Dispose();

            String expected = "<div class=\"input-group\">Content</div>";
            String actual = textWriter.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
