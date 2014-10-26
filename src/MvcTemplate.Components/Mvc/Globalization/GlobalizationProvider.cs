using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace MvcTemplate.Components.Mvc
{
    public class GlobalizationProvider : IGlobalizationProvider
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
                return Languages.Single(language => language.Culture == CultureInfo.CurrentUICulture);
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

        public GlobalizationProvider(String path)
        {
            XElement languagesXml = XElement.Load(path);
            languages = new Dictionary<String, Language>();

            foreach (XElement languageNode in languagesXml.Elements("language"))
            {
                Language language = new Language();
                language.Culture = new CultureInfo((String)languageNode.Attribute("culture"));
                language.IsDefault = (Boolean?)languageNode.Attribute("default") == true;
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
