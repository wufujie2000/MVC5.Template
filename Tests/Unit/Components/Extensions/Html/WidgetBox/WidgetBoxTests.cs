using NUnit.Framework;
using System;
using System.IO;
using Template.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class WidgetBoxTests
    {
        #region Constructor: WidgetBox(TextWriter writer, String iconClass, String title, String buttons)

        [Test]
        public void WidgetBox_WritesWidgetBox()
        {
            StringWriter textWriter = new StringWriter();
            using (new WidgetBox(textWriter, "Icon", "Title", "Buttons"))
                textWriter.Write("Test");

            String actual = textWriter.GetStringBuilder().ToString();
            String expected = String.Format("<div class=\"widget-box\">"
                + "<div class=\"widget-title\">"
                + "<span class=\"widget-title-icon\">"
                + "<i class=\"Icon\"></i></span>"
                + "<h5>Title</h5>"
                + "<div class=\"widget-title-buttons\">Buttons</div></div>"
                + "<div class=\"widget-content\">Test</div></div>",
                "icon-class",
                "Header title",
                "<button>Button html</button>",
                "<span>Content html</span>");

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanBeDisposedMultipleTimes()
        {
            StringWriter textWriter = new StringWriter();
            WidgetBox widgetBox = new WidgetBox(textWriter, "Icon", "Title", "Buttons");
            textWriter.Write("Test");
            widgetBox.Dispose();
            widgetBox.Dispose();

            String actual = textWriter.GetStringBuilder().ToString();
            String expected = String.Format("<div class=\"widget-box\">"
                + "<div class=\"widget-title\">"
                + "<span class=\"widget-title-icon\">"
                + "<i class=\"Icon\"></i></span>"
                + "<h5>Title</h5>"
                + "<div class=\"widget-title-buttons\">Buttons</div></div>"
                + "<div class=\"widget-content\">Test</div></div>",
                "icon-class",
                "Header title",
                "<button>Button html</button>",
                "<span>Content html</span>");

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
