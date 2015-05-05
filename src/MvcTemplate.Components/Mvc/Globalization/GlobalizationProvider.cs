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
        public Language[] Languages
        {
            get;
            private set;
        }
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
        private Dictionary<String, Language> LanguageDictionary
        {
            get;
            set;
        }

        public GlobalizationProvider(String path)
        {
            XElement languagesXml = XElement.Load(path);
            LanguageDictionary = new Dictionary<String, Language>();

            foreach (XElement languageNode in languagesXml.Elements("language"))
            {
                Language language = new Language();
                language.Culture = new CultureInfo((String)languageNode.Attribute("culture"));
                language.IsDefault = (Boolean?)languageNode.Attribute("default") == true;
                language.Abbrevation = (String)languageNode.Attribute("abbrevation");
                language.Name = (String)languageNode.Attribute("name");

                LanguageDictionary.Add(language.Abbrevation, language);
            }

            Languages = LanguageDictionary.Select(language => language.Value).ToArray();
            DefaultLanguage = Languages.Single(language => language.IsDefault);
        }

        public Language this[String abbrevation]
        {
            get
            {
                return LanguageDictionary[abbrevation];
            }
        }
    }
}
