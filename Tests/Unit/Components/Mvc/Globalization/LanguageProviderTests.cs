using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class LanguageProviderTests
    {
        private LanguageProvider provider;

        [SetUp]
        public void SetUp()
        {
            CreateLanguagesXml("Languages.xml");
            provider = new LanguageProvider("Languages.xml");
        }

        #region Constructor: LanguageProvider(String languagesPath)

        [Test]
        public void LanguageProvider_LoadsAllLanguages()
        {
            Language enLanguage = provider.Languages.First();
            Language ltLanguage = provider.Languages.Last();

            Assert.AreEqual(new CultureInfo("en-GB"), enLanguage.Culture);
            Assert.AreEqual("en", enLanguage.Abbrevation);
            Assert.AreEqual("English", enLanguage.Name);
            Assert.IsTrue(enLanguage.IsDefault);

            Assert.AreEqual(new CultureInfo("lt-LT"), ltLanguage.Culture);
            Assert.AreEqual("lt", ltLanguage.Abbrevation);
            Assert.AreEqual("Lietuvių", ltLanguage.Name);
            Assert.IsFalse(ltLanguage.IsDefault);
        }

        [Test]
        public void LanguageProvider_SetsDefaultLanguage()
        {
            Language actual = provider.DefaultLanguage;

            Assert.AreEqual(new CultureInfo("en-GB"), actual.Culture);
            Assert.AreEqual("en", actual.Abbrevation);
            Assert.AreEqual("English", actual.Name);
            Assert.IsTrue(actual.IsDefault);
        }

        #endregion

        #region Indexer: this[String abbrevation]

        [Test]
        public void Indexer_GetsLanguageByAbbrevation()
        {
            Language actual = provider["en"];

            Assert.AreEqual(new CultureInfo("en-GB"), actual.Culture);
            Assert.AreEqual("en", actual.Abbrevation);
            Assert.AreEqual("English", actual.Name);
            Assert.IsTrue(actual.IsDefault);
        }

        #endregion

        #region Test helpers

        private void CreateLanguagesXml(String languagesPath)
        {
            XElement languages = new XElement("Languages");
            XElement englishDefault = new XElement("Language");
            englishDefault.SetAttributeValue("name", "English");
            englishDefault.SetAttributeValue("culture", "en-GB");
            englishDefault.SetAttributeValue("abbrevation", "en");
            englishDefault.SetAttributeValue("is-default", "true");

            XElement lithuanian = new XElement("Language");
            lithuanian.SetAttributeValue("name", "Lietuvių");
            lithuanian.SetAttributeValue("culture", "lt-LT");
            lithuanian.SetAttributeValue("abbrevation", "lt");

            languages.Add(englishDefault);
            languages.Add(lithuanian);

            languages.Save(languagesPath);
        }

        #endregion
    }
}
