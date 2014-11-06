using MvcTemplate.Components.Extensions.Html;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class FormGroupTests
    {
        #region Constructor: FormGroup(TextWriter writer)

        [Test]
        public void FormGroup_WritesFormGroup()
        {
            StringWriter writer = new StringWriter(new StringBuilder());
            using (new FormGroup(writer)) writer.Write("Content");

            String expected = "<div class=\"form-group\">Content</div>";
            String actual = writer.GetStringBuilder().ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
