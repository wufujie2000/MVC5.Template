using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public abstract class AControllerTests
    {
        protected void ProtectsFromOverpostingId(Controller controller, String postMethod)
        {
            MethodInfo methodInfo = controller
                .GetType()
                .GetMethods()
                .First(method =>
                    method.Name == postMethod &&
                    method.GetCustomAttribute<HttpPostAttribute>() != null);

            CustomAttributeData actual = methodInfo
                .GetParameters()
                .First()
                .CustomAttributes
                .Single(attr => attr.AttributeType == typeof(BindAttribute));

            Assert.Equal("Exclude", actual.NamedArguments.Single().MemberName);
            Assert.Equal("Id", actual.NamedArguments.Single().TypedValue.Value);
        }
    }
}
