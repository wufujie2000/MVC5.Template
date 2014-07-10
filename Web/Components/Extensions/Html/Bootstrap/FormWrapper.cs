using System;
using System.IO;
using System.Web.Mvc;

namespace MvcTemplate.Components.Extensions.Html
{
    public class FormWrapper : IDisposable
    {
        protected TagBuilder wrapper;
        protected TextWriter writer;
        private Boolean disposed;

        public FormWrapper(TextWriter writer, String cssClass)
        {
            this.writer = writer;
            wrapper = new TagBuilder("div");
            wrapper.AddCssClass(cssClass.Trim());
            writer.Write(wrapper.ToString(TagRenderMode.StartTag));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;

            writer.Write(wrapper.ToString(TagRenderMode.EndTag));
            wrapper = null;
            writer = null;

            disposed = true;
        }

        public override String ToString()
        {
            return wrapper.ToString();
        }
    }
}
