using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Tests.Unit.Components.Mvc.Adapters
{
    public class RequiredAdapterModel
    {
        [Required]
        public String Id { get; set; }
    }
}
