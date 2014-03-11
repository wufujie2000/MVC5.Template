using NUnit.Framework;
using Template.Objects;

namespace Template.Tests.Tests.Objects.Shared
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
