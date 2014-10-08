using MvcTemplate.Components.Extensions.Html;
using NUnit.Framework;
using System;
using System.IO;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class WidgetBoxTests
    {
        #region Constructor: WidgetBox(TextWriter textWriter, String iconClass, String title, String buttons)

        [Test]
        public void WidgetBox_WritesWidgetBox()
        {
            StringWriter writer = new StringWriter();
            using (new WidgetBox(writer, "Icon", "Title", "Buttons")) writer.Write("Test");

            String actual = writer.GetStringBuilder().ToString();
            String expected = GetExpectedWidgetBoxHtml();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_ClosesWidgetDivs()
        {
            StringWriter writer = new StringWriter();
            using (new WidgetBox(writer, "Icon", "Title", "Buttons")) writer.Write("Test");

            String actual = writer.GetStringBuilder().ToString();
            String expected = GetExpectedWidgetBoxHtml();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            StringWriter writer = new StringWriter();
            WidgetBox widgetBox = new WidgetBox(writer, "Icon", "Title", "Buttons");
            writer.Write("Test");
            widgetBox.Dispose();
            widgetBox.Dispose();

            String actual = writer.GetStringBuilder().ToString();
            String expected = GetExpectedWidgetBoxHtml();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Test helpers

        private String GetExpectedWidgetBoxHtml()
        {
            return
                "<div class=\"widget-box\">" +
                    "<div class=\"widget-title\">" +
                        "<span class=\"widget-title-icon Icon\"></span>" +
                        "<h5>Title</h5>" +
                        "<div class=\"widget-title-buttons\">Buttons</div>" +
                    "</div>" +
                    "<div class=\"widget-content\">Test</div>" +
                "</div>";
        }

        #endregion
    }
}
