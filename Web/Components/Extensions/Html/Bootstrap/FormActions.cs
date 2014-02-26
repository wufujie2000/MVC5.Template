using System;
using System.IO;

namespace Template.Components.Extensions.Html
{
    public class FormActions : FormGroup
    {
        private FormWrapper formColumn;

        public FormActions(TextWriter writer)
            : base(writer)
        {
            formColumn = new FormWrapper(writer, "form-actions col-sm-9 col-md-9 col-lg-7");
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposed) return;
            formColumn.Dispose();
            base.Dispose(disposing);
        }
    }
}