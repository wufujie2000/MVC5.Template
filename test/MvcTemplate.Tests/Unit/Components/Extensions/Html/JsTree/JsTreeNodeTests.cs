using MvcTemplate.Components.Extensions.Html;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class JsTreeNodeTests
    {
        #region Constructor: JsTreeNode()

        [Fact]
        public void JsTreeNode_SetsIdToNull()
        {
            Assert.Null(new JsTreeNode().Id);
        }

        [Fact]
        public void JsTreeNode_SetsNameToNull()
        {
            Assert.Null(new JsTreeNode().Name);
        }

        [Fact]
        public void JsTreeNode_CreatesEmptyTree()
        {
            Assert.Empty(new JsTreeNode().Nodes);
        }

        #endregion

        #region Constructor: JsTreeNode(String name)

        [Fact]
        public void JsTreeNode_Name_SetsIdToNull()
        {
            Assert.Null(new JsTreeNode("Name").Id);
        }

        [Fact]
        public void JsTreeNode_Name_SetsName()
        {
            Assert.Equal("Name", new JsTreeNode("Name").Name);
        }

        [Fact]
        public void JsTreeNode_Name_CreatesEmptyTree()
        {
            Assert.Empty(new JsTreeNode("Name").Nodes);
        }

        #endregion

        #region Constructor: JsTreeNode(String id, String name)

        [Fact]
        public void JsTreeNode_Id_Name_SetsId()
        {
            Assert.Equal("Id", new JsTreeNode("Id", null).Id);
        }

        [Fact]
        public void JsTreeNode_Id_Name_SetsName()
        {
            Assert.Equal("Name", new JsTreeNode(null, "Name").Name);
        }

        [Fact]
        public void JsTreeNode_Id_Name_CreatesEmptyTree()
        {
            Assert.Empty(new JsTreeNode("Id", "Name").Nodes);
        }

        #endregion
    }
}
