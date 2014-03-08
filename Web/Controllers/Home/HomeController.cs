using System.Web.Mvc;
using Template.Components.Security;
using Template.Components.Services;

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
