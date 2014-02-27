using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using Template.Components.Extensions.Html;

namespace Template.Tests.Tests.Components.Extensions.Html
{
    [TestFixture]
    public class FormColumnTests
    {
        #region Constructor: FormColumn(Object innerHtml)

        [Test]
        public void FormColumn_FormsColumnWrapper()
        {
            var expected = new StringBuilder();
            var expectedWriter = new StringWriter(expected);
            var formWrapper = new FormWrapper(expectedWriter, "col-sm-9 col-md-9 col-lg-5");
            expectedWriter.Write("TestContent");
            formWrapper.Dispose();

            Assert.AreEqual(expected.ToString(), new FormColumn("TestContent").ToString());
        }

        #endregion

        #region Constructor: FormColumn(TextWriter writer)

        [Test]
        public void FormColumn_WritesFormColumnWrapper()
        {
            var actual = new StringBuilder();
            var actualWriter = new StringWriter(actual);
            var formColumn = new FormColumn(actualWriter);
            actualWriter.Write("TestContent");
            formColumn.Dispose();
            formColumn.Dispose();

            var expected = new StringBuilder();
            var expectedWriter = new StringWriter(expected);
            var formWrapper = new FormWrapper(expectedWriter, "col-sm-9 col-md-9 col-lg-5");
            expectedWriter.Write("TestContent");
            formWrapper.Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion
    }
}
