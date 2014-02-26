using System.IO;

namespace Template.Components.Extensions.Html
{
    public class FormGroup : FormWrapper
    {
        public FormGroup(TextWriter writer)
            : base(writer, "form-group")
        {
        }
    }
}
