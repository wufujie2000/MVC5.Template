using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Administration
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
                    "AdministrationMultilingual",
                    "{language}/Administration/{controller}/{action}/{id}",
                    new { area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "lt-LT" },
                    new[] { "MvcTemplate.Controllers.Administration" });

            context
                .MapRoute(
                    "Administration",
                    "Administration/{controller}/{action}/{id}",
                    new { language = "en-GB", area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "en-GB" },
                    new[] { "MvcTemplate.Controllers.Administration" });
        }
    }
}
