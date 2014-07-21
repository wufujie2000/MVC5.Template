using Moq;
using MvcTemplate.Components.Mvc;
using System.Collections.Generic;
using System.Globalization;

namespace MvcTemplate.Tests.Helpers
{
    public class LanguageProviderMock
    {
        public Mock<ILanguageProvider> Mock
        {
            get;
            private set;
        }
        public ILanguageProvider Provider
        {
            get;
            private set;
        }

        public LanguageProviderMock()
        {
            Mock = new Mock<ILanguageProvider>();
            Provider = Mock.Object;
            SetUpMock();
        }

        private void SetUpMock()
        {
            List<Language> languages = new List<Language>();
            languages.Add(new Language()
            {
                Culture = new CultureInfo("en-GB"),
                Abbrevation = "en",
                IsDefault = true,
                Name = "English"
            });

            languages.Add(new Language()
            {
                Culture = new CultureInfo("lt-LT"),
                Abbrevation = "lt",
                IsDefault = false,
                Name = "Lietuvių"
            });

            Mock.SetupGet(mock => mock.DefaultLanguage).Returns(languages[0]);
            Mock.SetupGet(mock => mock["en"]).Returns(languages[0]);
            Mock.SetupGet(mock => mock["lt"]).Returns(languages[1]);
            Mock.Setup(mock => mock.Languages).Returns(languages);
        }
    }
}
