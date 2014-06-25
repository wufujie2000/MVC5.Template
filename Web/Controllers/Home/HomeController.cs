using System.Web.Mvc;
using Template.Components.Security;
using Template.Resources.Shared;
using Template.Services;

namespace Template.Controllers.Home
{
    [AllowUnauthorized]
    public class HomeController : ServicedController<IHomeService>
    {
        public HomeController(IHomeService service)
            : base(service)
        {
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Error()
        {
            Service.AlertMessages.AddError(Messages.SystemError);

            return View();
        }

        [HttpGet]
        public ViewResult NotFound()
        {
            Service.AlertMessages.AddError(Messages.PageNotFound);

            return View();
        }

        [HttpGet]
        public ViewResult Unauthorized()
        {
            Service.AlertMessages.AddError(Messages.Unauthorized);

            return View();
        }
    }
}
