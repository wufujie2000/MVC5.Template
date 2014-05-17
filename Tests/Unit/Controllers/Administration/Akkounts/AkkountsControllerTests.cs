using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Template.Controllers.Administration;
using Template.Objects;
using Template.Services;

namespace Template.Tests.Unit.Controllers.Administration
{
    [TestFixture]
    public class AkkountsControllerTests
    {
        private Mock<AkkountsController> controllerMock;
        private Mock<IAkkountsService> serviceMock;
        private AkkountsController controller;
        private AkkountView akkount;

        [SetUp]
        public void SetUp()
        {
		    // TODO: Add view creation from ObjectFactory
            serviceMock = new Mock<IAkkountsService>();
            serviceMock.Setup(mock => mock.CanEdit(akkount)).Returns(true);
            serviceMock.Setup(mock => mock.CanCreate(akkount)).Returns(true);
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(akkount);
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(true);
            controllerMock = new Mock<AkkountsController>(serviceMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            controller = controllerMock.Object;
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsModelsView()
        {
            IEnumerable<AkkountView> expected = new[] { akkount };
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected.AsQueryable());
            Object actual = (controller.Index() as ViewResult).Model;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsEmptyAkkountView()
        {
            AkkountView actual = (controller.Create() as ViewResult).Model as AkkountView;

            Assert.IsNotNull(actual.Id);
            // TODO: Add asserts for actual properties
        }

        #endregion

        #region Method: Create(AkkountView akkount)

        [Test]
        public void Create_ProtectsFromOverpostingId()
        {
            MethodInfo createMethod = controller
                .GetType()
                .GetMethods()
                .First(method =>
                    method.Name == "Create" &&
                    method.GetCustomAttribute<HttpPostAttribute>() != null);

            CustomAttributeData customParameterAttribute = createMethod.GetParameters().First().CustomAttributes.First();

            Assert.AreEqual(typeof(BindAttribute), customParameterAttribute.AttributeType);
            Assert.AreEqual("Exclude", customParameterAttribute.NamedArguments.First().MemberName);
            Assert.AreEqual("Id", customParameterAttribute.NamedArguments.First().TypedValue.Value);
        }

        [Test]
        public void Create_ReturnsEmptyViewIfCanNotCreate()
        {
            serviceMock.Setup(mock => mock.CanCreate(akkount)).Returns(false);

            Assert.IsNull((controller.Create(akkount) as ViewResult).Model);
        }

        [Test]
        public void Create_CallsServiceCreate()
        {
            controller.Create(akkount);

            serviceMock.Verify(mock => mock.Create(akkount), Times.Once());
        }

        [Test]
        public void Create_AfterCreateRedirectsToIndex()
        {
            RedirectToRouteResult result = controller.Create(akkount) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsAkkountView()
        {
            AkkountView actual = (controller.Details("Test") as ViewResult).Model as AkkountView;

            Assert.AreEqual(akkount, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsAkkountView()
        {
            AkkountView actual = (controller.Edit("Test") as ViewResult).Model as AkkountView;

            Assert.AreEqual(akkount, actual);
        }

        #endregion

        #region Method: Edit(AkkountView akkount)

        [Test]
        public void Edit_ReturnsEmptyViewIfCanNotEdit()
        {
            serviceMock.Setup(mock => mock.CanEdit(akkount)).Returns(false);

            Assert.IsNull((controller.Edit(akkount) as ViewResult).Model);
        }

        [Test]
        public void Edit_CallsServiceEdit()
        {
            controller.Edit(akkount);

            serviceMock.Verify(mock => mock.Edit(akkount), Times.Once());
        }

        [Test]
        public void Edit_RedirectsToIndex()
        {
            RedirectToRouteResult result = controller.Edit(akkount) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsViewWithDeleteModel()
        {
            AkkountView actual = (controller.Delete("Test") as ViewResult).Model as AkkountView;

            Assert.AreEqual(akkount, actual);
        }

        #endregion

        #region Method: DeleteConfirmed(String id)

        [Test]
        public void DeleteConfirmed_ReturnsEmptyViewIfCanNotDelete()
        {
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(false);

            Assert.IsNull((controller.DeleteConfirmed("Test") as ViewResult).Model);
        }

        [Test]
        public void DeleteConfirmed_CallsServiceDelete()
        {
            controller.DeleteConfirmed("Test");

            serviceMock.Verify(mock => mock.Delete("Test"), Times.Once());
        }

        [Test]
        public void DeleteConfirmed_RedirectsToIndex()
        {
            RedirectToRouteResult result = controller.DeleteConfirmed("Test") as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion
    }
}
