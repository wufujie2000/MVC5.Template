using System.Web.Mvc;
using Template.Components.Security;
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
            Service.AddSystemErrorMessage();

            return View();
        }

        [HttpGet]
        public ViewResult NotFound()
        {
            Service.AddPageNotFoundMessage();

            return View();
        }

        [HttpGet]
        public ViewResult Unauthorized()
        {
            Service.AddUnauthorizedMessage();

            return View();
        }
    }
}
