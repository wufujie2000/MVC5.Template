using System;

namespace MvcTemplate.Components.Security
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AuthorizeAsAttribute : Attribute
    {
        public String Action { get; set; }

        public AuthorizeAsAttribute(String action)
        {
            Action = action;
        }
    }
}
