using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Shared;
using NUnit.Framework;
using System;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class MinValueAttributeTests
    {
        private MinValueAttribute attribute;

        [SetUp]
        public void SetUp()
        {
            attribute = new MinValueAttribute(12.56);
        }

        #region Constructor: MinValueAttribute(Int32 minimum)

        [Test]
        public void MinValueAttribute_SetsMinimumFromInteger()
        {
            Decimal actual = new MinValueAttribute(10).Minimum;
            Decimal expected = 10M;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: MinValueAttribute(Double minimum)

        [Test]
        public void MinValueAttribute_SetsMinimumFromDouble()
        {
            Decimal actual = attribute.Minimum;
            Decimal expected = 12.56M;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: FormatErrorMessage(String name)

        [Test]
        public void FormatErrorMessage_FormatsErrorMessageForInteger()
        {
            MinValueAttribute attribute = new MinValueAttribute(10);

            String expected = String.Format(Validations.FiledMustBeGreaterOrEqualTo, "Sum", attribute.Minimum);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatErrorMessage_FormatsErrorMessageForDouble()
        {
            String expected = String.Format(Validations.FiledMustBeGreaterOrEqualTo, "Sum", attribute.Minimum);
            String actual = new MinValueAttribute(12.56).FormatErrorMessage("Sum");

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: IsValid(Object value)

        [Test]
        public void IsValid_NullValueIsValid()
        {
            Assert.IsTrue(attribute.IsValid(null));
        }

        [Test]
        public void IsValid_IsValidStringValue()
        {
            Assert.IsTrue(attribute.IsValid("100"));
        }

        [Test]
        public void IsValid_IsNotValidStringValue()
        {
            Assert.IsFalse(attribute.IsValid("1"));
        }

        [Test]
        public void IsValid_LowerValueIsNotValid()
        {
            Assert.IsFalse(attribute.IsValid(12.559));
        }

        [Test]
        public void IsValid_EqualValueIsValid()
        {
            Assert.IsTrue(attribute.IsValid(12.56));
        }

        [Test]
        public void IsValid_GreaterValueIsValid()
        {
            Assert.IsTrue(attribute.IsValid(13));
        }

        [Test]
        public void IsValid_NotDecimalValueIsNotValid()
        {
            Assert.IsFalse(attribute.IsValid("12.56M"));
        }

        #endregion
    }
}
