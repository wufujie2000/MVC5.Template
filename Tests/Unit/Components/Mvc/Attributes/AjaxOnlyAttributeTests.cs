using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc.Attributes
{
    [TestFixture]
    public class AjaxOnlyAttributeTests
    {
        #region Method: IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)

        [Test]
        public void IsValidForRequest_IsFalseIfNormalRequest()
        {
            ControllerContext context = new ControllerContext();
            context.HttpContext = new HttpMock().HttpContextBase;
            context.HttpContext.Request["X-Requested-With"].Returns(String.Empty);

            Assert.IsFalse(new AjaxOnlyAttribute().IsValidForRequest(context, null));
        }

        [Test]
        public void IsValidForRequest_IsTrueIfAjaxRequest()
        {
            ControllerContext context = new ControllerContext();
            context.HttpContext = new HttpMock().HttpContextBase;
            context.HttpContext.Request["X-Requested-With"].Returns("XMLHttpRequest");

            Assert.IsTrue(new AjaxOnlyAttribute().IsValidForRequest(context, null));
        }

        #endregion
    }
}
