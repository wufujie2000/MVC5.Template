using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
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
            Decimal actual = new MinValueAttribute(12.56).Minimum;
            Decimal expected = 12.56M;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: FormatErrorMessage(String name)

        [Test]
        public void FormatErrorMessage_FormatsErrorMessageForInteger()
        {
            attribute = new MinValueAttribute(10);

            String expected = String.Format(Validations.FieldMustBeGreaterOrEqualTo, "Sum", attribute.Minimum);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatErrorMessage_FormatsErrorMessageForDouble()
        {
            attribute = new MinValueAttribute(12.56);

            String expected = String.Format(Validations.FieldMustBeGreaterOrEqualTo, "Sum", attribute.Minimum);
            String actual = attribute.FormatErrorMessage("Sum");

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
        public void IsValid_StringValueIsValid()
        {
            Assert.IsTrue(attribute.IsValid("100"));
        }

        [Test]
        public void IsValid_StringValueIsNotValid()
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
