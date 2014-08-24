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
            TagBuilder titleIcon = new TagBuilder("i");
            widgetContent = new TagBuilder("div");
            widgetBox = new TagBuilder("div");
            writer = textWriter;

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
            widgetContent = null;
            widgetBox = null;

            disposed = true;
        }
    }
}
