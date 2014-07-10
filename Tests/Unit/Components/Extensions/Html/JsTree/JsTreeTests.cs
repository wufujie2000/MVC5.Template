using MvcTemplate.Components.Extensions.Html;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class JsTreeTests
    {
        #region Constructor: JsTree()

        [Test]
        public void JsTree_CreatesEmptyTree()
        {
            CollectionAssert.IsEmpty(new JsTree().Nodes);
        }

        [Test]
        public void JsTree_CreatesUnselectedTree()
        {
            CollectionAssert.IsEmpty(new JsTree().SelectedIds);
        }

        #endregion
    }
}
