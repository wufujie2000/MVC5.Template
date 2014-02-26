using System.IO;

namespace Template.Components.Extensions.Html
{
    public class InputGroup : FormWrapper
    {
        public InputGroup(TextWriter writer)
            : base(writer, "input-group")
        {
        }
    }
}
