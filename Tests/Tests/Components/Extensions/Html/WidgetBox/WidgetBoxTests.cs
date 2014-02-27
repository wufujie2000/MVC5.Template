using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Template.Components.Extensions.Html;

namespace Template.Tests.Tests.Components.Extensions.Html
{
    [TestFixture]
    public class WidgetBoxTests
    {
        #region Constructor: WidgetBox(TextWriter writer, String iconClass, String title, String buttons)

        [Test]
        public void WidgetBox_WritesWidgetBox()
        {
            var widgetBox = new TagBuilder("div");
            var widgetTitle = new TagBuilder("div");
            var titleIconSpan = new TagBuilder("span");
            var titleIcon = new TagBuilder("i");
            var titleHeader = new TagBuilder("h5");
            var titleButtons = new TagBuilder("div");
            var widgetContent = new TagBuilder("div");

            widgetBox.AddCssClass("widget-box");
            widgetTitle.AddCssClass("widget-title");
            widgetContent.AddCssClass("widget-content");
            titleIconSpan.AddCssClass("widget-title-icon");
            titleButtons.AddCssClass("widget-title-buttons");
            titleIcon.AddCssClass("Icon");
            titleButtons.InnerHtml = "Buttons";
            titleHeader.InnerHtml = "Title";

            titleIconSpan.InnerHtml = titleIcon.ToString();
            widgetTitle.InnerHtml = String.Format("{0}{1}{2}", titleIconSpan, titleHeader, titleButtons);

            var expected = widgetBox.ToString(TagRenderMode.StartTag);
            expected += widgetTitle.ToString() + widgetContent.ToString(TagRenderMode.StartTag);
            expected += "Test";
            expected += widgetBox.ToString(TagRenderMode.EndTag);
            expected += widgetContent.ToString(TagRenderMode.EndTag);
           
            var actual = new StringBuilder();
            var actualWriter = new StringWriter(actual);
            var widget = new WidgetBox(actualWriter, "Icon", "Title", "Buttons");
            actualWriter.Write("Test");
            widget.Dispose();
            widget.Dispose();

            Assert.AreEqual(expected, actual.ToString());
        }

        #endregion
    }
}
