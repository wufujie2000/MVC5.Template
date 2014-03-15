using NUnit.Framework;
using System;
using System.Reflection;
using System.Linq;
using System.Web.Mvc;

namespace Template.Tests.Tests.Controllers
{
    [TestFixture]
    public class SecurityTests
    {
        #region ValidateAntiForgeryToken

        [Test]
        public void AllControllerPostMethods_HasValidateAntiForgeryToken()
        {
            var postMethods = Assembly
                .Load("Template.Controllers")
                .GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttribute<HttpPostAttribute>() != null);

            foreach (var method in postMethods)
                Assert.IsNotNull(method.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        }

        #endregion
    }
}
