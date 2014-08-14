using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace MvcTemplate.Components.Mvc
{
    public class LanguageProvider : ILanguageProvider
    {
        private Dictionary<String, Language> languages;

        public Language DefaultLanguage
        {
            get;
            private set;
        }
        public Language CurrentLanguage
        {
            get
            {
                return Languages.Single(language => language.Culture == CultureInfo.CurrentCulture);
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = value.Culture;
                Thread.CurrentThread.CurrentUICulture = value.Culture;
            }
        }
        public IEnumerable<Language> Languages
        {
            get
            {
                return languages.Select(language => language.Value);
            }
        }

        public LanguageProvider(String languagesPath)
        {
            XElement languagesXml = XElement.Load(languagesPath);
            languages = new Dictionary<String, Language>();

            foreach (XElement languageNode in languagesXml.Elements("Language"))
            {
                Language language = new Language();
                language.Culture = new CultureInfo((String)languageNode.Attribute("culture"));
                language.IsDefault = (Boolean?)languageNode.Attribute("is-default") == true;
                language.Abbrevation = (String)languageNode.Attribute("abbrevation");
                language.Name = (String)languageNode.Attribute("name");

                languages.Add(language.Abbrevation, language);
            }

            DefaultLanguage = languages.Single(language => language.Value.IsDefault).Value;
        }

        public Language this[String abbrevation]
        {
            get
            {
                return languages[abbrevation];
            }
        }
    }
}
