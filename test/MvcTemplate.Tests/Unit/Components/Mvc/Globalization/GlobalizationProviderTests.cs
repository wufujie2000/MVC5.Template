using MvcTemplate.Components.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class GlobalizationProviderTests
    {
        private GlobalizationProvider provider;

        static GlobalizationProviderTests()
        {
            CreateGlobalizationXml("Globalization.xml");
        }
        public GlobalizationProviderTests()
        {
            provider = new GlobalizationProvider("Globalization.xml");
        }

        #region Property: CurrentLanguage

        [Fact]
        public void CurrentLanguage_GetsCurrentLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = provider["en"].Culture;

            Language actual = provider.CurrentLanguage;
            Language expected = provider["en"];

            Assert.Same(expected, actual);
        }

        [Fact]
        public void CurrentLanguage_SetsCurrentLanguage()
        {
            provider.CurrentLanguage = provider.Languages.Last();

            CultureInfo expectedCulture = provider.Languages.Last().Culture;
            CultureInfo actualUICulture = CultureInfo.CurrentUICulture;
            CultureInfo actualCulture = CultureInfo.CurrentCulture;

            Assert.Same(expectedCulture, actualUICulture);
            Assert.Same(expectedCulture, actualCulture);
        }

        #endregion

        #region Constructor: GlobalizationProvider(String path)

        [Fact]
        public void GlobalizationProvider_LoadsAllLanguages()
        {
            Language ltLanguage = provider.Languages.First();
            Language enLanguage = provider.Languages.Last();

            Assert.Equal(new CultureInfo("en-GB"), enLanguage.Culture);
            Assert.Equal("en", enLanguage.Abbrevation);
            Assert.Equal("English", enLanguage.Name);
            Assert.True(enLanguage.IsDefault);

            Assert.Equal(new CultureInfo("lt-LT"), ltLanguage.Culture);
            Assert.Equal("lt", ltLanguage.Abbrevation);
            Assert.Equal("Lietuvių", ltLanguage.Name);
            Assert.False(ltLanguage.IsDefault);
        }

        [Fact]
        public void GlobalizationProvider_SetsDefaultLanguage()
        {
            Language actual = provider.DefaultLanguage;

            Assert.Equal(new CultureInfo("en-GB"), actual.Culture);
            Assert.Equal("en", actual.Abbrevation);
            Assert.Equal("English", actual.Name);
            Assert.True(actual.IsDefault);
        }

        #endregion

        #region Indexer: this[String abbrevation]

        [Fact]
        public void Indexer_GetsLanguageByAbbrevation()
        {
            Language actual = provider["en"];

            Assert.Equal(new CultureInfo("en-GB"), actual.Culture);
            Assert.Equal("en", actual.Abbrevation);
            Assert.Equal("English", actual.Name);
            Assert.True(actual.IsDefault);
        }

        #endregion

        #region Test helpers

        private static void CreateGlobalizationXml(String path)
        {
            XElement english = new XElement("language");
            english.SetAttributeValue("name", "English");
            english.SetAttributeValue("culture", "en-GB");
            english.SetAttributeValue("abbrevation", "en");
            english.SetAttributeValue("default", "true");

            XElement lithuanian = new XElement("language");
            lithuanian.SetAttributeValue("name", "Lietuvių");
            lithuanian.SetAttributeValue("culture", "lt-LT");
            lithuanian.SetAttributeValue("abbrevation", "lt");

            XElement globalization = new XElement("globalization");
            globalization.Add(lithuanian);
            globalization.Add(english);

            globalization.Save(path);
        }

        #endregion
    }
}
