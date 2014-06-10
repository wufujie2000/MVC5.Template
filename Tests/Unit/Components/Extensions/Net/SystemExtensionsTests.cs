using NUnit.Framework;
using System;
using Template.Components.Extensions.Net;

namespace Template.Tests.Unit.Components.Extensions.Net
{
    [TestFixture]
    public class SystemExtensionsTests
    {
        #region Extension method: LimitTo(this Int32 number, Int32 lowerBound, Int32 upperBound)

        [Test]
        public void LimitTo_LimitsToLowerBound()
        {
            Assert.AreEqual(1, 0.LimitTo(1, 3));
        }

        [Test]
        public void LimitTo_LimitsLowerBound()
        {
            Assert.AreEqual(1, 1.LimitTo(1, 3));
        }

        [Test]
        public void LimitTo_DoesNotLimit()
        {
            Assert.AreEqual(2, 2.LimitTo(1, 3));
        }

        [Test]
        public void LimitTo_LimitsUpperBound()
        {
            Assert.AreEqual(3, 3.LimitTo(1, 3));
        }

        [Test]
        public void LimitTo_LimitsToUpperBound()
        {
            Assert.AreEqual(3, 4.LimitTo(1, 3));
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
