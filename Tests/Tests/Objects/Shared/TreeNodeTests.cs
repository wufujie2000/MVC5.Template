using NUnit.Framework;
using Template.Objects;

namespace Template.Tests.Tests.Objects
{
    [TestFixture]
    public class TreeNodeTests
    {
        #region Constructor: TreeNode()

        [Test]
        public void TreeNode_ParameterlessConstructor_IdIsNull()
        {
            Assert.IsNull(new TreeNode().Id);
        }

        [Test]
        public void TreeNode_ParameterlessConstructor_NameIsNull()
        {
            Assert.IsNull(new TreeNode().Name);
        }

        [Test]
        public void TreeNode_ParameterlessConstructor_NodesAreEmpty()
        {
            CollectionAssert.IsEmpty(new TreeNode().Nodes);
        }

        #endregion

        #region Constructor: TreeNode(String name)

        [Test]
        public void TreeNode_NameConstructor_IdIsNull()
        {
            Assert.IsNull(new TreeNode().Id);
        }

        [Test]
        public void TreeNode_NameConstructor_SetsName()
        {
            Assert.AreEqual("Name", new TreeNode("Name").Name);
        }

        [Test]
        public void TreeNode_NameConstructor_NodesAreEmpty()
        {
            CollectionAssert.IsEmpty(new TreeNode().Nodes);
        }

        #endregion

        #region Constructor: TreeNode(String name)

        [Test]
        public void TreeNode_IdAndNameConstructor_SetsId()
        {
            Assert.AreEqual("Id", new TreeNode("Id", null).Id);
        }

        [Test]
        public void TreeNode_IdAndNameConstructor_SetsName()
        {
            Assert.AreEqual("Name", new TreeNode("Name").Name);
        }

        [Test]
        public void TreeNode_IdAndNameConstructor_NodesAreEmpty()
        {
            CollectionAssert.IsEmpty(new TreeNode().Nodes);
        }

        #endregion
    }
}
