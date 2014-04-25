using Template.Objects;

namespace Template.Tests.Objects
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
