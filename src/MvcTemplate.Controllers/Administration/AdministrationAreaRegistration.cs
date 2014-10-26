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
                    "{language}/{area}/{controller}/{action}/{id}",
                    new { area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "lt", area = "Administration" },
                    new[] { "MvcTemplate.Controllers.Administration" });

            context
                .MapRoute(
                    "Administration",
                    "{area}/{controller}/{action}/{id}",
                    new { language = "en", area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "en", area = "Administration" },
                    new[] { "MvcTemplate.Controllers.Administration" });
        }
    }
}
