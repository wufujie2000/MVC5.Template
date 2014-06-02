using NUnit.Framework;
using Template.Components.Extensions.Html;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class MenuTests
    {
        #region Constructor: Menu()

        [Test]
        public void Menu_SubmenusAreEmpty()
        {
            CollectionAssert.IsEmpty(new Menu().Submenus);
        }

        #endregion
    }
}
