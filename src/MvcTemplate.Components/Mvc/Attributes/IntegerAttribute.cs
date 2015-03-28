using MvcTemplate.Resources.Form;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IntegerAttribute : ValidationAttribute
    {
        public IntegerAttribute() : base(() => Validations.FieldMustBeInteger)
        {
        }

        public override Boolean IsValid(Object value)
        {
            if (value == null)
                return true;

            return Regex.IsMatch(value.ToString(), "^[=-]?\\d+$");
        }
    }
}
