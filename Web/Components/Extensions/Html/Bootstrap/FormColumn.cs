using System;
using System.Web.Mvc;

namespace Template.Components.Extensions.Html
{
    public class FormColumn : FormWrapper
    {
        public FormColumn(Object innerHtml)
            : base("col-sm-9 col-md-9 col-lg-5")
        {
            wrapper.InnerHtml = innerHtml.ToString();
        }
        public FormColumn(ViewContext viewContext)
            : base(viewContext, "col-sm-9 col-md-9 col-lg-5")
        {
        }
    }
}
