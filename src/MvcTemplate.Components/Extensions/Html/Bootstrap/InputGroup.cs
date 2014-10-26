using System.IO;

namespace MvcTemplate.Components.Extensions.Html
{
    public class InputGroup : FormWrapper
    {
        public InputGroup(TextWriter writer)
            : base(writer, "input-group")
        {
        }
    }
}
