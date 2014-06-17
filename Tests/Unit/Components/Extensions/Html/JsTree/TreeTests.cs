using NUnit.Framework;
using Template.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class TreeTests
    {
        #region Constructor: Tree()

        [Test]
        public void Tree_NodesAreEmpty()
        {
            CollectionAssert.IsEmpty(new Tree().Nodes);
        }

        [Test]
        public void Tree_SelectedIdsAreEmpty()
        {
            CollectionAssert.IsEmpty(new Tree().SelectedIds);
        }

        #endregion
    }
}
