using System;

namespace MvcTemplate.Components.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowUnauthorizedAttribute : Attribute
    {
    }
}
