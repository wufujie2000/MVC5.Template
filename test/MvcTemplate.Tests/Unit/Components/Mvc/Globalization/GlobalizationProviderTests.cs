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
    public class GlobalizationProviderTests
    {
        private GlobalizationProvider provider;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            CreateGlobalizationXml("Globalization.xml");
        }

        [SetUp]
        public void SetUp()
        {
            provider = new GlobalizationProvider("Globalization.xml");
        }

        #region Property: CurrentLanguage

        [Test]
        public void CurrentLanguage_GetsCurrentLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = provider["en"].Culture;

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

        #region Constructor: GlobalizationProvider(String path)

        [Test]
        public void GlobalizationProvider_LoadsAllLanguages()
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
        public void GlobalizationProvider_SetsDefaultLanguage()
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

        private void CreateGlobalizationXml(String path)
        {
            XElement globalization = new XElement("globalization");
            XElement english = new XElement("language");
            english.SetAttributeValue("name", "English");
            english.SetAttributeValue("culture", "en-GB");
            english.SetAttributeValue("abbrevation", "en");
            english.SetAttributeValue("default", "true");

            XElement lithuanian = new XElement("language");
            lithuanian.SetAttributeValue("name", "Lietuvių");
            lithuanian.SetAttributeValue("culture", "lt-LT");
            lithuanian.SetAttributeValue("abbrevation", "lt");

            globalization.Add(english);
            globalization.Add(lithuanian);

            globalization.Save(path);
        }

        #endregion
    }
}
