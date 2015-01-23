using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using NUnit.Framework;
using System;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class MaxValueAttributeTests
    {
        private MaxValueAttribute attribute;

        [SetUp]
        public void SetUp()
        {
            attribute = new MaxValueAttribute(12.56);
        }

        #region Constructor: MaxValueAttribute(Int32 maximum)

        [Test]
        public void MaxValueAttribute_SetsMaximumFromInteger()
        {
            Decimal actual = new MaxValueAttribute(10).Maximum;
            Decimal expected = 10M;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: MaxValueAttribute(Double maximum)

        [Test]
        public void MaxValueAttribute_SetsMaximumFromDouble()
        {
            Decimal actual = new MaxValueAttribute(12.56).Maximum;
            Decimal expected = 12.56M;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: FormatErrorMessage(String name)

        [Test]
        public void FormatErrorMessage_FormatsErrorMessageForInteger()
        {
            attribute = new MaxValueAttribute(10);

            String expected = String.Format(Validations.FieldMustBeLessOrEqualTo, "Sum", attribute.Maximum);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormatErrorMessage_FormatsErrorMessageForDouble()
        {
            attribute = new MaxValueAttribute(13.44);

            String expected = String.Format(Validations.FieldMustBeLessOrEqualTo, "Sum", attribute.Maximum);
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
            Assert.IsTrue(attribute.IsValid("5"));
        }

        [Test]
        public void IsValid_StringValueIsNotValid()
        {
            Assert.IsFalse(attribute.IsValid("100"));
        }

        [Test]
        public void IsValid_GreaterValueIsNotValid()
        {
            Assert.IsFalse(attribute.IsValid(13));
        }

        [Test]
        public void IsValid_EqualValueIsValid()
        {
            Assert.IsTrue(attribute.IsValid(12.56));
        }

        [Test]
        public void IsValid_LowerValueIsValid()
        {
            Assert.IsTrue(attribute.IsValid(12.559));
        }

        [Test]
        public void IsValid_NotDecimalValueIsNotValid()
        {
            Assert.IsFalse(attribute.IsValid("12.56M"));
        }

        #endregion
    }
}
