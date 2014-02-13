using Template.Components.Security;
using Template.Components.Services;
using System.Web.Mvc;

namespace Template.Controllers.Home
{
    [AllowUnauthorized]
    public class HomeController : ServicedController<HomeService>
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            Service.AddSystemErrorMessage();
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            Service.AddPageNotFoundMessage();
            return View();
        }

        [HttpGet]
        public ActionResult Unauthorized()
        {
            Service.AddUnauthorizedMessage();
            return View();
        }
    }
}
