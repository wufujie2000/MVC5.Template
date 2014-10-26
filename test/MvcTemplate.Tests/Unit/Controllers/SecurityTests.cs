using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class SecurityTests
    {
        #region ValidateAntiForgeryToken

        [Test]
        public void AllControllerPostMethods_HasValidateAntiForgeryToken()
        {
            IEnumerable<MethodInfo> postMethods = Assembly
                .Load("MvcTemplate.Controllers")
                .GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttribute<HttpPostAttribute>() != null);

            foreach (MethodInfo method in postMethods)
                Assert.IsNotNull(method.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        }

        #endregion
    }
}
