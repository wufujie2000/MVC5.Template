using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Helpers;
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
            HttpMock http = new HttpMock();
            http.HttpRequestMock.Setup(mock => mock["X-Requested-With"]).Returns(String.Empty);

            ControllerContext context = new ControllerContext();
            context.HttpContext = http.HttpContextBase;

            Assert.IsFalse(new AjaxOnlyAttribute().IsValidForRequest(context, null));
        }

        [Test]
        public void IsValidForRequest_IsTrueIfAjaxRequest()
        {
            HttpMock http = new HttpMock();
            http.HttpRequestMock.Setup(mock => mock["X-Requested-With"]).Returns("XMLHttpRequest");

            ControllerContext context = new ControllerContext();
            context.HttpContext = http.HttpContextBase;

            Assert.IsTrue(new AjaxOnlyAttribute().IsValidForRequest(context, null));
        }

        #endregion
    }
}
