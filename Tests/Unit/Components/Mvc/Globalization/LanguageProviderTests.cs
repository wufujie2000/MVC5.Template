using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
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

        #region Property: CurrentLanguage

        [Test]
        public void CurrentLanguage_GetsCurrentLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = provider["en"].Culture;
            Thread.CurrentThread.CurrentCulture = provider["en"].Culture;

            Language actual = provider.CurrentLanguage;
            Language expected = provider["en"];

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void CurrentLanguage_SetsCurrentLanguage()
        {
            provider.CurrentLanguage = provider.Languages.Last();

            CultureInfo expectedCulture = provider.Languages.Last().Culture;
            CultureInfo actualUICulture = CultureInfo.CurrentUICulture;
            CultureInfo actualCulture = CultureInfo.CurrentCulture;

            Assert.AreSame(expectedCulture, actualUICulture);
            Assert.AreSame(expectedCulture, actualCulture);
        }

        #endregion

        #region Constructor: LanguageProvider(String path)

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
            XElement english = new XElement("Language");
            english.SetAttributeValue("name", "English");
            english.SetAttributeValue("culture", "en-GB");
            english.SetAttributeValue("abbrevation", "en");
            english.SetAttributeValue("is-default", "true");

            XElement lithuanian = new XElement("Language");
            lithuanian.SetAttributeValue("name", "Lietuvių");
            lithuanian.SetAttributeValue("culture", "lt-LT");
            lithuanian.SetAttributeValue("abbrevation", "lt");

            languages.Add(english);
            languages.Add(lithuanian);

            languages.Save(languagesPath);
        }

        #endregion
    }
}
