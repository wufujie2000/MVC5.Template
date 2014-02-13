using System;
using System.Web.Mvc;

namespace Template.Components.Extensions.Html
{
    public class FormActions : FormGroup
    {
        private FormWrapper formColumn;

        public FormActions(ViewContext viewContext)
            : base(viewContext)
        {
            formColumn = new FormWrapper(viewContext, "form-actions col-sm-9 col-md-9 col-lg-7");
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposed) return;
            formColumn.Dispose();
            base.Dispose(disposing);
        }
    }
}