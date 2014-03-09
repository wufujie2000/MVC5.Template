using System;
using System.Web.Mvc;

namespace Template.Controllers.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration
    {
        public override String AreaName
        {
            get
            {
                return "Administration";
            }
        }
        
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context
                .MapRoute(
                    "Administration",
                    "{language}/Administration/{controller}/{action}/{id}",
                    new { area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "lt-LT" },
                    new[] { "Template.Controllers.Administration" });

            context
                .MapRoute(
                    "AdministrationDefaultLang",
                    "Administration/{controller}/{action}/{id}",
                    new { language = "en-GB", area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "en-GB" },
                    new[] { "Template.Controllers.Administration" });
        }
    }
}
