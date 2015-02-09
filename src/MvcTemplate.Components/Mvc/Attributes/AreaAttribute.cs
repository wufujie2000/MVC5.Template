using System;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AreaAttribute : Attribute
    {
        public String Name { get; set; }

        public AreaAttribute(String name)
        {
            Name = name;
        }
    }
}
