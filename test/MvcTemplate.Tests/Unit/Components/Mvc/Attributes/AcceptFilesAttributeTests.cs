using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Web;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class AcceptFilesAttributeTests
    {
        private AcceptFilesAttribute attribute;

        public AcceptFilesAttributeTests()
        {
            attribute = new AcceptFilesAttribute(".docx,.xlsx");
        }

        #region AcceptFilesAttribute(String extensions)

        [Fact]
        public void AcceptFilesAttribute_SetsExtensions()
        {
            String actual = new AcceptFilesAttribute(".docx,.xlsx").Extensions;
            String expected = ".docx,.xlsx";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormatErrorMessage(String name)

        [Fact]
        public void FormatErrorMessage_ForProperty()
        {
            attribute = new AcceptFilesAttribute(".docx,.xlsx");

            String expected = String.Format(Validations.AcceptFiles, "File", attribute.Extensions);
            String actual = attribute.FormatErrorMessage("File");

            Assert.Equal(expected, actual);
        }

        #endregion

        #region IsValid(Object value)

        [Fact]
        public void IsValid_Null()
        {
            Assert.True(attribute.IsValid(null));
        }

        [Fact]
        public void IsValid_NotFileReturnsFalse()
        {
            Assert.False(attribute.IsValid("100"));
        }

        [Fact]
        public void IsValid_FileWithoutNameReturnsFalse()
        {
            HttpPostedFileBase fileBase = Substitute.For<HttpPostedFileBase>();
            fileBase.FileName.ReturnsNull();

            Assert.False(attribute.IsValid(fileBase));
        }

        [Fact]
        public void IsValid_AnyFileWithoutNameReturnsFalse()
        {
            HttpPostedFileBase[] fileBases = { Substitute.For<HttpPostedFileBase>(), Substitute.For<HttpPostedFileBase>() };
            fileBases[0].FileName.Returns("File.docx");
            fileBases[1].FileName.ReturnsNull();

            Assert.False(attribute.IsValid(fileBases));
        }

        [Theory]
        [InlineData("")]
        [InlineData(".")]
        [InlineData(".doc")]
        [InlineData("docx")]
        [InlineData(".docx.doc")]
        public void IsValid_DifferentExtensionReturnsFalse(String fileName)
        {
            HttpPostedFileBase fileBase = Substitute.For<HttpPostedFileBase>();
            fileBase.FileName.Returns(fileName);

            Assert.False(attribute.IsValid(fileBase));
        }

        [Theory]
        [InlineData("")]
        [InlineData(".")]
        [InlineData(".doc")]
        [InlineData("docx")]
        [InlineData(".docx.doc")]
        public void IsValid_DifferentExtensionsReturnsFalse(String fileName)
        {
            HttpPostedFileBase[] fileBases = { Substitute.For<HttpPostedFileBase>(), Substitute.For<HttpPostedFileBase>() };
            fileBases[0].FileName.Returns("File.docx");
            fileBases[1].FileName.Returns(fileName);

            Assert.False(attribute.IsValid(fileBases));
        }

        [Theory]
        [InlineData(".docx")]
        [InlineData(".xlsx")]
        [InlineData("docx.docx")]
        [InlineData("docx..docx")]
        [InlineData("xlsx.doc.xlsx")]
        public void IsValid_Extension(String fileName)
        {
            HttpPostedFileBase fileBase = Substitute.For<HttpPostedFileBase>();
            fileBase.FileName.Returns(fileName);

            Assert.True(attribute.IsValid(fileBase));
        }

        [Theory]
        [InlineData("docx.docx", ".docx")]
        [InlineData("docx..docx", ".xlsx")]
        [InlineData(".xlsx", "docx..docx")]
        [InlineData(".docx", "xlsx.doc.xlsx")]
        [InlineData("xlsx.doc.xlsx", ".docx.docx")]
        public void IsValid_Exntesions(String firstFileName, String secondFileName)
        {
            HttpPostedFileBase[] fileBases = { Substitute.For<HttpPostedFileBase>(), Substitute.For<HttpPostedFileBase>() };
            fileBases[1].FileName.Returns(secondFileName);
            fileBases[0].FileName.Returns(firstFileName);

            Assert.True(attribute.IsValid(fileBases));
        }

        #endregion
    }
}
