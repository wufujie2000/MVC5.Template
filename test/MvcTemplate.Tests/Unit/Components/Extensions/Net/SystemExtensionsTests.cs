using MvcTemplate.Components.Extensions.Net;
using NUnit.Framework;
using System;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Net
{
    [TestFixture]
    public class SystemExtensionsTests
    {
        #region Extension method: LimitTo(this Int32 number, Int32 lowerBound, Int32 upperBound)

        [Test]
        [TestCase(0, 1, 3, 1)]
        [TestCase(1, 1, 3, 1)]
        [TestCase(2, 1, 3, 2)]
        [TestCase(3, 1, 3, 3)]
        [TestCase(4, 1, 3, 3)]
        public void LimitTo_LimitsTo(Int32 number, Int32 lowerBound, Int32 upperBound, Int32 expected)
        {
            Int32 actual = number.LimitTo(lowerBound, upperBound);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: Split(this String str, String separator)

        [Test]
        public void Split_SplitsByStringSeperator()
        {
            CollectionAssert.AreEqual("T,,E,D,,B".Split(new[] { "," }, StringSplitOptions.None), "T,,E,D,,B".Split(","));
        }

        #endregion

        #region Extension method: Split(this String str, String separator, Int32 count)

        [Test]
        public void Split_SplitsByStringAndCount()
        {
            CollectionAssert.AreEqual("T,,E,D,,B".Split(new[] { "," }, 3, StringSplitOptions.None), "T,,E,D,,B".Split(",", 3));
        }

        #endregion

        #region Extension method: Split(this String str, Char separator, Int32 count)

        [Test]
        public void Split_SplitsByCharAndCount()
        {
            CollectionAssert.AreEqual("M,,C,E,,D".Split(new[] { ',' }, 3), "M,,C,E,,D".Split(',', 3));
        }

        #endregion

        #region Extension method: Split(this String str, Char seperator, StringSplitOptions options)

        [Test]
        public void Split_SplitsByCharAndOptions()
        {
            foreach (StringSplitOptions option in Enum.GetValues(typeof(StringSplitOptions)))
                CollectionAssert.AreEqual("M,B,,D".Split(new[] { ',' }, option), "M,B,,D".Split(',', option));
        }

        #endregion

        #region Extension method: Split(this String str, String seperator, StringSplitOptions options)

        [Test]
        public void Split_SplitsByStringAndOptions()
        {
            foreach (StringSplitOptions option in Enum.GetValues(typeof(StringSplitOptions)))
                CollectionAssert.AreEqual("M,B,,D".Split(new[] { "," }, option), "M,B,,D".Split(",", option));
        }

        #endregion

        #region Extension method: Split(this String str, Char seperator, Int32 count, StringSplitOptions options)

        [Test]
        public void Split_SplitsByCharCountAndOptions()
        {
            foreach (StringSplitOptions option in Enum.GetValues(typeof(StringSplitOptions)))
                CollectionAssert.AreEqual("M,B,,D".Split(new[] { ',' }, 3, option), "M,B,,D".Split(',', 3, option));
        }

        #endregion

        #region Extension method: Split(this String str, String seperator, Int32 count, StringSplitOptions options)

        [Test]
        public void Split_SplitsByStringCountAndOptions()
        {
            foreach (StringSplitOptions option in Enum.GetValues(typeof(StringSplitOptions)))
                CollectionAssert.AreEqual("M,,E,B,,D".Split(new[] { "," }, 3, option), "M,,E,B,,D".Split(",", 3, option));
        }

        #endregion
    }
}
