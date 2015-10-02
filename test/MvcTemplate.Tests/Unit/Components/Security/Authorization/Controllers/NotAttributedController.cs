using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class NotAttributedController : Controller
    {
        [HttpGet]
        [ExcludeFromCodeCoverage]
        public ViewResult Action()
        {
            return null;
        }
    }
}
