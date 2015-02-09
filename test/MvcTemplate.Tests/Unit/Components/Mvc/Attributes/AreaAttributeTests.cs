using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class AreaAttributeTests
    {
        #region Constructor: AreaAttribute(String name)

        [Test]
        public void AreaAttribute_SetsName()
        {
            String actual = new AreaAttribute("Name").Name;
            String expected = "Name";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
