using NUnit.Framework;
using Template.Objects;

namespace Template.Tests.Unit.Objects
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
