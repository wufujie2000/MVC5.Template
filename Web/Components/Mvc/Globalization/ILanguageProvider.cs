using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public interface ILanguageProvider
    {
        Language DefaultLanguage { get; }
        IEnumerable<Language> Languages { get; }

        Language this[String abbrevation] { get; }
    }
}
