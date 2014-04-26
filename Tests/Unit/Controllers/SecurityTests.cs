using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Template.Tests.Unit.Controllers
{
    [TestFixture]
    public class SecurityTests
    {
        #region ValidateAntiForgeryToken

        [Test]
        public void AllControllerPostMethods_HasValidateAntiForgeryToken()
        {
            IEnumerable<MethodInfo> postMethods = Assembly
                .Load("Template.Controllers")
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
