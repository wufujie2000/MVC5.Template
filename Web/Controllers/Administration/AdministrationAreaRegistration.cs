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
                    new { area = "Administration" },
                    new { language = "lt", action = "(?!Index).*" },
                    new[] { "MvcTemplate.Controllers.Administration" });

            context
                .MapRoute(
                    "AdministrationMultilingualIndex",
                    "{language}/Administration/{controller}/{action}",
                    new { area = "Administration", action = "Index" },
                    new { language = "lt", action = "Index" },
                    new[] { "MvcTemplate.Controllers.Administration" });

            context
                .MapRoute(
                    "Administration",
                    "Administration/{controller}/{action}/{id}",
                    new { language = "en", area = "Administration" },
                    new { language = "en", action = "(?!Index).*" },
                    new[] { "MvcTemplate.Controllers.Administration" });

            context
                .MapRoute(
                    "AdministrationIndex",
                    "Administration/{controller}/{action}",
                    new { language = "en", area = "Administration", action = "Index"},
                    new { language = "en", action = "Index" },
                    new[] { "MvcTemplate.Controllers.Administration" });
        }
    }
}
