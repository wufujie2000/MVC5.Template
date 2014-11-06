using MvcTemplate.Components.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class AjaxOnlyAttributeTests
    {
        #region Method: IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)

        [Test]
        [TestCase("", false)]
        [TestCase("XMLHttpRequest", true)]
        public void IsValidForRequest_ValidatesAjaxRequests(String headerValue, Boolean expected)
        {
            ControllerContext context = new ControllerContext();
            context.HttpContext = HttpContextFactory.CreateHttpContextBase();
            context.HttpContext.Request["X-Requested-With"].Returns(headerValue);

            Boolean actual = new AjaxOnlyAttribute().IsValidForRequest(context, null);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
