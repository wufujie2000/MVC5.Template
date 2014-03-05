using NUnit.Framework;
using System.IO;
using System.Text;
using Template.Components.Extensions.Html;

namespace Template.Tests.Tests.Components.Extensions.Html
{
    [TestFixture]
    public class FormActionsTests
    {
        #region Constructor: FormActions(TextWriter writer)

        [Test]
        public void FormActions_WritesFormActions()
        {
            var expected = new StringBuilder();
            var expectedWriter = new StringWriter(expected);
            var group = new FormGroup(expectedWriter);
            var wrapper = new FormWrapper(expectedWriter, "form-actions col-sm-9 col-md-9 col-lg-7");
            expectedWriter.Write("Content");
            wrapper.Dispose();
            group.Dispose();

            var actual = new StringBuilder();
            var actualWriter = new StringWriter(actual);
            var formActions = new FormActions(actualWriter);
            actualWriter.Write("Content");
            formActions.Dispose();
            formActions.Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion
    }
}
