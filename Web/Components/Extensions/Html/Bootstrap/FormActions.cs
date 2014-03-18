using System;
using System.IO;

namespace Template.Components.Extensions.Html
{
    public class FormActions : FormGroup
    {
        private FormWrapper formWrapper;

        public FormActions(TextWriter writer)
            : base(writer)
        {
            formWrapper = new FormWrapper(writer, "form-actions col-sm-12 col-md-12 col-lg-7");
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposed) return;
            formWrapper.Dispose();
            base.Dispose(disposing);
        }
    }
}