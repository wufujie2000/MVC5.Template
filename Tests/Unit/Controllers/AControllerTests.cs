using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public abstract class AControllerTests
    {
        protected void ProtectsFromOverpostingId(Controller controller, String postMethod)
        {
            MethodInfo createMethod = controller
                .GetType()
                .GetMethods()
                .First(method =>
                    method.Name == postMethod &&
                    method.GetCustomAttribute<HttpPostAttribute>() != null);

            CustomAttributeData customParameterAttribute = createMethod.GetParameters().First().CustomAttributes.First();

            Assert.AreEqual(typeof(BindAttribute), customParameterAttribute.AttributeType);
            Assert.AreEqual("Exclude", customParameterAttribute.NamedArguments.First().MemberName);
            Assert.AreEqual("Id", customParameterAttribute.NamedArguments.First().TypedValue.Value);
        }
    }
}
