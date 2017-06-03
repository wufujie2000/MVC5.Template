using System;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AreaAttribute : Attribute
    {
        public String Name { get; }

        public AreaAttribute(String name)
        {
            Name = name;
        }
    }
}
