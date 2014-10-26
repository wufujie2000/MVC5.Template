using MvcTemplate.Components.Extensions.Html;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class JsTreeNodeTests
    {
        #region Constructor: JsTreeNode()

        [Test]
        public void JsTreeNode_SetsIdToNull()
        {
            Assert.IsNull(new JsTreeNode().Id);
        }

        [Test]
        public void JsTreeNode_SetsNameToNull()
        {
            Assert.IsNull(new JsTreeNode().Name);
        }

        [Test]
        public void JsTreeNode_CreatesEmptyTree()
        {
            CollectionAssert.IsEmpty(new JsTreeNode().Nodes);
        }

        #endregion

        #region Constructor: JsTreeNode(String name)

        [Test]
        public void JsTreeNode_Name_SetsIdToNull()
        {
            Assert.IsNull(new JsTreeNode("Name").Id);
        }

        [Test]
        public void JsTreeNode_Name_SetsName()
        {
            Assert.AreEqual("Name", new JsTreeNode("Name").Name);
        }

        [Test]
        public void JsTreeNode_Name_CreatesEmptyTree()
        {
            CollectionAssert.IsEmpty(new JsTreeNode("Name").Nodes);
        }

        #endregion

        #region Constructor: JsTreeNode(String id, String name)

        [Test]
        public void JsTreeNode_Id_Name_SetsId()
        {
            Assert.AreEqual("Id", new JsTreeNode("Id", null).Id);
        }

        [Test]
        public void JsTreeNode_Id_Name_SetsName()
        {
            Assert.AreEqual("Name", new JsTreeNode(null, "Name").Name);
        }

        [Test]
        public void JsTreeNode_Id_Name_CreatesEmptyTree()
        {
            CollectionAssert.IsEmpty(new JsTreeNode("Id", "Name").Nodes);
        }

        #endregion
    }
}
