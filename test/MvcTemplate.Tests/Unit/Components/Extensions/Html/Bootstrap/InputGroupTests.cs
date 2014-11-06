using MvcTemplate.Components.Extensions.Html;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class InputGroupTests
    {
        #region Constructor: InputGroup(TextWriter writer)

        [Test]
        public void InputGroup_WritesInputGroup()
        {
            StringWriter writer = new StringWriter(new StringBuilder());
            using (new InputGroup(writer)) writer.Write("Content");

            String expected = "<div class=\"input-group\">Content</div>";
            String actual = writer.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
