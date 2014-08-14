using MvcTemplate.Components.Mvc;
using NSubstitute;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace MvcTemplate.Tests.Helpers
{
    public class LanguageProviderMock
    {
        public ILanguageProvider Provider
        {
            get;
            private set;
        }

        public LanguageProviderMock()
        {
            Provider = Substitute.For<ILanguageProvider>();
            SetUpMock();
        }

        private void SetUpMock()
        {
            List<Language> languages = new List<Language>()
            {
                new Language()
                {
                    Culture = new CultureInfo("en-GB"),
                    Abbrevation = "en",
                    IsDefault = true,
                    Name = "English"
                },
                new Language()
                {
                    Culture = new CultureInfo("lt-LT"),
                    Abbrevation = "lt",
                    IsDefault = false,
                    Name = "Lietuvių"
                }
            };

            Provider.When(language => language.CurrentLanguage = Arg.Any<Language>()).Do((value) =>
            {
                Thread.CurrentThread.CurrentCulture = value.Arg<Language>().Culture;
                Thread.CurrentThread.CurrentUICulture = value.Arg<Language>().Culture;
            });
            Provider.CurrentLanguage.Returns(languages[0]);
            Provider.DefaultLanguage.Returns(languages[0]);
            Provider.Languages.Returns(languages);
            Provider["en"].Returns(languages[0]);
            Provider["lt"].Returns(languages[1]);
        }
    }
}
