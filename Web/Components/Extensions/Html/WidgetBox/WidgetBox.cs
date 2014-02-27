using System;
using System.IO;
using System.Web.Mvc;

namespace Template.Components.Extensions.Html
{
    public class WidgetBox : IDisposable
    {
        private Boolean disposed;
        private TextWriter writer;
        private TagBuilder widgetBox;
        private TagBuilder widgetContent;

        public WidgetBox(TextWriter writer, String iconClass, String title, String buttons)
        {
            this.writer = writer;
            widgetBox = new TagBuilder("div");
            var widgetTitle = new TagBuilder("div");
            var titleIconSpan = new TagBuilder("span");
            var titleIcon = new TagBuilder("i");
            var titleHeader = new TagBuilder("h5");
            var titleButtons = new TagBuilder("div");
            widgetContent = new TagBuilder("div");

            widgetBox.AddCssClass("widget-box");
            widgetTitle.AddCssClass("widget-title");
            widgetContent.AddCssClass("widget-content");
            titleIconSpan.AddCssClass("widget-title-icon");
            titleButtons.AddCssClass("widget-title-buttons");
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
            disposed = true;
        }
    }
}
