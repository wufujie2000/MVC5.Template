using NUnit.Framework;
using System.IO;
using System.Text;
using System.Web.Mvc;
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
            var expected = new TagBuilder("div");
            expected.AddCssClass("Test");

            var actual = new FormWrapper(" Test ");

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Constructor: FormWrapper(ViewContext viewContext, String cssClass)

        [Test]
        public void FormWrapper_WritesWrapper()
        {
            var expected = new TagBuilder("div");
            expected.InnerHtml = "TestContent";
            expected.AddCssClass("Test");

            var actual = new StringBuilder();
            var writer = new StringWriter(actual);
            var formWrapper = new FormWrapper(writer, " Test ");
            writer.Write("TestContent");
            formWrapper.Dispose();
            formWrapper.Dispose();

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion
    }
}
