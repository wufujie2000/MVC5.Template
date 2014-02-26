using NUnit.Framework;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Template.Components.Extensions.Html;

namespace Template.Tests.Tests.Components.Extensions.Html.Bootstrap
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

            Assert.AreEqual(expected.ToString(), new FormWrapper(" Test ").ToString());
        }

        #endregion

        #region Constructor: FormWrapper(ViewContext viewContext, String cssClass)

        [Test]
        public void FormWrapper_WritesWrapper()
        {
            var actual = new StringBuilder();
            var writer = new StringWriter(actual);
            var formWrapper = new FormWrapper(writer, " Test ");
            writer.Write("TestContent");
            formWrapper.Dispose();
            formWrapper.Dispose();

            var expected = new TagBuilder("div");
            expected.InnerHtml = "TestContent";
            expected.AddCssClass("Test");

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion
    }
}
