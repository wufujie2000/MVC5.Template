using System.Web.Mvc;

namespace Template.Components.Extensions.Html
{
    public class FormGroup : FormWrapper
    {
        public FormGroup(ViewContext viewContext)
            : base(viewContext, "form-group")
        {
        }
    }
}
