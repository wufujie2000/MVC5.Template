using System;
using System.IO;
using System.Web.Mvc;

namespace MvcTemplate.Components.Extensions.Html
{
    public class WidgetBox : IDisposable
    {
        private TagBuilder widgetContent;
        private TagBuilder widgetBox;
        private TextWriter writer;
        private Boolean disposed;

        public WidgetBox(TextWriter textWriter, String iconClass, String title, String buttons)
        {
            TagBuilder titleIconSpan = new TagBuilder("span");
            TagBuilder titleButtons = new TagBuilder("div");
            TagBuilder widgetTitle = new TagBuilder("div");
            TagBuilder titleHeader = new TagBuilder("h5");
            widgetContent = new TagBuilder("div");
            widgetBox = new TagBuilder("div");
            writer = textWriter;

            titleIconSpan.AddCssClass("widget-title-icon " + iconClass);
            titleButtons.AddCssClass("widget-title-buttons");
            widgetContent.AddCssClass("widget-content");
            widgetTitle.AddCssClass("widget-title");
            widgetBox.AddCssClass("widget-box");
            titleButtons.InnerHtml = buttons;
            titleHeader.InnerHtml = title;

            widgetTitle.InnerHtml = String.Format("{0}{1}{2}", titleIconSpan, titleHeader, titleButtons);

            writer.Write(widgetBox.ToString(TagRenderMode.StartTag));
            writer.Write(widgetTitle.ToString(TagRenderMode.Normal));
            writer.Write(widgetContent.ToString(TagRenderMode.StartTag));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;

            writer.Write(widgetBox.ToString(TagRenderMode.EndTag));
            writer.Write(widgetContent.ToString(TagRenderMode.EndTag));

            disposed = true;
        }
    }
}
