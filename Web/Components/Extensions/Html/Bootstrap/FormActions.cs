using System;
using System.IO;

namespace Template.Components.Extensions.Html
{
    public class FormActions : FormGroup
    {
        private FormWrapper formWrapper;
        private Boolean disposed;

        public FormActions(TextWriter writer, String cssClass)
            : base(writer)
        {
            formWrapper = new FormWrapper(writer, cssClass);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposed) return;

            formWrapper.Dispose();
            formWrapper = null;
            disposed = true;

            base.Dispose(disposing);
        }
    }
}