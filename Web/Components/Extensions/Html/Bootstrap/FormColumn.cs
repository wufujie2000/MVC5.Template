using System;
using System.IO;

namespace Template.Components.Extensions.Html
{
    public class FormColumn : FormWrapper
    {
        public FormColumn(Object innerHtml)
            : base("col-sm-9 col-md-9 col-lg-5")
        {
            wrapper.InnerHtml = innerHtml.ToString();
        }
        public FormColumn(TextWriter writer)
            : base(writer, "col-sm-9 col-md-9 col-lg-5")
        {
        }
    }
}
