using System.Web.Mvc;

namespace Template.Components.Extensions.Html
{
    public class InputGroup : FormWrapper
    {
        public InputGroup(ViewContext viewContext)
            : base(viewContext, "input-group")
        {
        }
    }
}
