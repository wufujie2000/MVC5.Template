using System;
using System.IO;
using System.Web.Mvc;

namespace Template.Components.Extensions.Html
{
    public class FormWrapper : IDisposable
    {
        protected Boolean disposed;
        protected TextWriter writer;
        protected TagBuilder wrapper;

        public FormWrapper(String cssClass)
        {
            wrapper = new TagBuilder("div");
            wrapper.AddCssClass(cssClass.Trim());
        }
        public FormWrapper(TextWriter writer, String cssClass)
            : this(cssClass)
        {
            this.writer = writer;
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
            disposed = true;
        }

        public override string ToString()
        {
            return wrapper.ToString();
        }
    }
}
