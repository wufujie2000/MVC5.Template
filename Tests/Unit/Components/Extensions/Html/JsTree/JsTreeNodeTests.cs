using NUnit.Framework;
using Template.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class JsTreeNodeTests
    {
        #region Constructor: JsTreeNode()

        [Test]
        public void JsTreeNode_ParameterlessConstructor_IdIsNull()
        {
            Assert.IsNull(new JsTreeNode().Id);
        }

        [Test]
        public void JsTreeNode_ParameterlessConstructor_NameIsNull()
        {
            Assert.IsNull(new JsTreeNode().Name);
        }

        [Test]
        public void JsTreeNode_ParameterlessConstructor_NodesAreEmpty()
        {
            CollectionAssert.IsEmpty(new JsTreeNode().Nodes);
        }

        #endregion

        #region Constructor: JsTreeNode(String name)

        [Test]
        public void JsTreeNode_NameConstructor_IdIsNull()
        {
            Assert.IsNull(new JsTreeNode().Id);
        }

        [Test]
        public void JsTreeNode_NameConstructor_SetsName()
        {
            Assert.AreEqual("Name", new JsTreeNode("Name").Name);
        }

        [Test]
        public void JsTreeNode_NameConstructor_NodesAreEmpty()
        {
            CollectionAssert.IsEmpty(new JsTreeNode().Nodes);
        }

        #endregion

        #region Constructor: JsTreeNode(String name)

        [Test]
        public void JsTreeNode_IdAndNameConstructor_SetsId()
        {
            Assert.AreEqual("Id", new JsTreeNode("Id", null).Id);
        }

        [Test]
        public void JsTreeNode_IdAndNameConstructor_SetsName()
        {
            Assert.AreEqual("Name", new JsTreeNode("Name").Name);
        }

        [Test]
        public void JsTreeNode_IdAndNameConstructor_NodesAreEmpty()
        {
            CollectionAssert.IsEmpty(new JsTreeNode().Nodes);
        }

        #endregion
    }
}
