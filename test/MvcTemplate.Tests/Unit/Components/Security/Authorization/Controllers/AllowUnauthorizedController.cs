using MvcTemplate.Components.Security;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [AllowUnauthorized]
    public abstract class AllowUnauthorizedController : AuthorizedController
    {
    }
}
