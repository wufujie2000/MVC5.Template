using System.IO;

namespace MvcTemplate.Components.Extensions.Html
{
    public class FormGroup : FormWrapper
    {
        public FormGroup(TextWriter writer)
            : base(writer, "form-group")
        {
        }
    }
}
