using NUnit.Framework;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Template.Components.Extensions.Html;

namespace Template.Tests.Tests.Components.Extensions.Html.Bootstrap
{
    [TestFixture]
    public class FormGroupTests
    {
        #region Constructor: FormGroup(TextWriter writer)

        [Test]
        public void FormGroup_WritesFormGroup()
        {
            var actual = new StringBuilder();
            var writer = new StringWriter(actual);
            var formGroup = new FormGroup(writer);
            writer.Write("TestContent");
            formGroup.Dispose();
            formGroup.Dispose();

            var expected = new TagBuilder("div");
            expected.InnerHtml = "TestContent";
            expected.AddCssClass("form-group");

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion
    }
}
