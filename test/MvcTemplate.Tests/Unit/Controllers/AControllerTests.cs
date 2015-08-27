using MvcTemplate.Controllers;
using NSubstitute;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public abstract class AControllerTests
    {
        protected void ReturnsCurrentAccountId(BaseController controller, String id)
        {
            controller.When(sub => { String get = sub.CurrentAccountId; }).DoNotCallBase();
            controller.CurrentAccountId.Returns(id);
        }

        protected void ProtectsFromOverposting(Controller controller, String postMethod, String value)
        {
            MethodInfo methodInfo = controller
                .GetType()
                .GetMethods()
                .First(method =>
                    method.Name == postMethod &&
                    method.IsDefined(typeof(HttpPostAttribute), false));

            CustomAttributeData actual = methodInfo
                .GetParameters()
                .First()
                .CustomAttributes
                .Single(attr => attr.AttributeType == typeof(BindAttribute));

            Assert.Equal(value, actual.NamedArguments.Single().TypedValue.Value);
            Assert.Equal("Exclude", actual.NamedArguments.Single().MemberName);
        }

        protected RedirectToRouteResult NotEmptyView(BaseController controller, Object model)
        {
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary());
            controller.When(sub => sub.NotEmptyView(model)).DoNotCallBase();
            controller.NotEmptyView(model).Returns(result);

            return result;
        }

        protected RedirectToRouteResult RedirectToDefault(BaseController controller)
        {
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary());
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(result);

            return result;
        }
        protected RedirectToRouteResult RedirectToNotFound(BaseController controller)
        {
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary());
            controller.When(sub => sub.RedirectToNotFound()).DoNotCallBase();
            controller.RedirectToNotFound().Returns(result);

            return result;
        }
        protected RedirectToRouteResult RedirectIfAuthorized(BaseController controller, String action)
        {
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary());
            controller.When(sub => sub.RedirectIfAuthorized(action)).DoNotCallBase();
            controller.RedirectIfAuthorized(action).Returns(result);

            return result;
        }
    }
}
