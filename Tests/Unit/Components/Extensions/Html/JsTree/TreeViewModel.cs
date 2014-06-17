using Template.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    public class TreeViewModel
    {
        public Tree Tree { get; set; }

        public TreeViewModel()
        {
            Tree = new Tree();
        }
    }
}
