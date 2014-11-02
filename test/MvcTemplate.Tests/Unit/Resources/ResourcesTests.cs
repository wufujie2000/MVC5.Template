using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace MvcTemplate.Tests.Unit.Resources
{
    [TestFixture]
    public class ResourcesTests
    {
        [Test]
        public void Resources_HasAllTranslations()
        {
            IEnumerable<Language> languages = GlobalizationProviderFactory.CreateProvider().Languages;
            IEnumerable<Type> resourceTypes = Assembly
                .Load("MvcTemplate.Resources")
                .GetTypes()
                .Where(type => type.Namespace.StartsWith("MvcTemplate.Resources."));

            foreach (Type type in resourceTypes)
            {
                IEnumerable<String> resourceKeys = Enumerable.Empty<String>();
                ResourceManager manager = new ResourceManager(type);

                foreach (Language language in languages)
                {
                    ResourceSet set = manager.GetResourceSet(language.Culture, true, true);
                    resourceKeys = resourceKeys.Union(set.Cast<DictionaryEntry>().Select(resource => resource.Key.ToString()));
                    resourceKeys = resourceKeys.Distinct();
                }

                foreach (Language language in languages)
                {
                    ResourceSet set = manager.GetResourceSet(language.Culture, true, true);

                    foreach (String key in resourceKeys)
                        Assert.IsNotNullOrEmpty(
                            set.GetString(key),
                            String.Format("{0}, does not have translation for '{1}' in {2} language.",
                                type.FullName,
                                key,
                                language.Culture.EnglishName));
                }
            }
        }
    }
}
