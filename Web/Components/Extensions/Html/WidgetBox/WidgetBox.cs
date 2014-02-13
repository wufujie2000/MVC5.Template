using System;
using System.Web.Mvc;

namespace Template.Components.Extensions.Html
{
    public class WidgetBox : IDisposable
    {
        private Boolean disposed;
        private ViewContext context;
        private TagBuilder widgetBox;
        private TagBuilder widgetContent;

        public WidgetBox(ViewContext viewContext, String iconClass, String title, String buttons)
        {
            this.context = viewContext;
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

            context.Writer.Write(widgetBox.ToString(TagRenderMode.StartTag));
            context.Writer.Write(widgetTitle.ToString());
            context.Writer.Write(widgetContent.ToString(TagRenderMode.StartTag));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;
            context.Writer.Write(widgetBox.ToString(TagRenderMode.EndTag));
            context.Writer.Write(widgetContent.ToString(TagRenderMode.EndTag));
            disposed = true;
        }
    }
}
