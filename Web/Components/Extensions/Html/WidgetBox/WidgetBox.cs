using System;
using System.IO;
using System.Web.Mvc;

namespace MvcTemplate.Components.Extensions.Html
{
    public class WidgetBox : IDisposable
    {
        private Boolean disposed;
        private TextWriter writer;
        private TagBuilder widgetBox;
        private TagBuilder widgetContent;

        public WidgetBox(TextWriter writer, String iconClass, String title, String buttons)
        {
            TagBuilder titleIconSpan = new TagBuilder("span");
            TagBuilder titleButtons = new TagBuilder("div");
            TagBuilder widgetTitle = new TagBuilder("div");
            TagBuilder titleHeader = new TagBuilder("h5");
            TagBuilder titleIcon = new TagBuilder("i");
            widgetContent = new TagBuilder("div");
            widgetBox = new TagBuilder("div");
            this.writer = writer;

            titleButtons.AddCssClass("widget-title-buttons");
            titleIconSpan.AddCssClass("widget-title-icon");
            widgetContent.AddCssClass("widget-content");
            widgetTitle.AddCssClass("widget-title");
            widgetBox.AddCssClass("widget-box");
            titleIcon.AddCssClass(iconClass);
            titleButtons.InnerHtml = buttons;
            titleHeader.InnerHtml = title;

            titleIconSpan.InnerHtml = titleIcon.ToString();
            widgetTitle.InnerHtml = String.Format("{0}{1}{2}", titleIconSpan, titleHeader, titleButtons);

            writer.Write(widgetBox.ToString(TagRenderMode.StartTag));
            writer.Write(widgetTitle.ToString());
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
            widgetContent = null;
            widgetBox = null;

            disposed = true;
        }
    }
}
