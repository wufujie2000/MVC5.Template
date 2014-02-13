using System;
using System.Web.Mvc;

namespace Template.Components.Extensions.Html
{
    public class FormWrapper : IDisposable
    {
        protected Boolean disposed;
        protected TagBuilder wrapper;
        protected ViewContext context;

        public FormWrapper(String cssClass)
        {
            wrapper = new TagBuilder("div");
            wrapper.AddCssClass(cssClass.Trim());
        }
        public FormWrapper(ViewContext viewContext, String cssClass)
            : this(cssClass)
        {
            context = viewContext;
            context.Writer.Write(wrapper.ToString(TagRenderMode.StartTag));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;
            context.Writer.Write(wrapper.ToString(TagRenderMode.EndTag));
            disposed = true;
        }

        public override string ToString()
        {
            return wrapper.ToString();
        }
    }
}
