using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Services;
using Template.Tests.Objects.Components.Services;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class ServiceFactoryTests
    {
        #region Static method: CreateService<T>(ModelStateDictionary modelState) where T : BaseService

        [Test]
        public void Instance_CreatesServiceOfGivenType()
        {
            Assert.AreEqual(typeof(BaseServiceStub), ServiceFactory.CreateService<BaseServiceStub>(new ModelStateDictionary()).GetType());
        }

        [Test]
        public void Instance_CreatesServiceWithModelState()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("TestKey", "TestError");
            var service = ServiceFactory.CreateService<BaseServiceStub>(modelState);

            Assert.AreEqual(1, service.AlertMessages.Count());
            Assert.AreEqual("TestKey", service.AlertMessages.First().Key);
            Assert.AreEqual("TestError", service.AlertMessages.First().Message);
        }

        #endregion
    }
}
