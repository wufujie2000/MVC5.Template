using MvcTemplate.Components.Extensions.Html;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class JsTreeTests
    {
        #region Constructor: JsTree()

        [Test]
        public void JsTree_NodesAreEmpty()
        {
            CollectionAssert.IsEmpty(new JsTree().Nodes);
        }

        [Test]
        public void JsTree_SelectedIdsAreEmpty()
        {
            CollectionAssert.IsEmpty(new JsTree().SelectedIds);
        }

        #endregion
    }
}
