using System;

namespace MvcTemplate.Components.Security
{
    public sealed class AuthorizeAsAttribute : Attribute
    {
        public String Action { get; set; }

        public AuthorizeAsAttribute(String action)
        {
            Action = action;
        }
    }
}
