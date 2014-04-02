using NUnit.Framework;
using Template.Objects;

namespace Template.Tests.Unit.Objects
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
