using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class TrimmingModelBinderTests
    {
        private NameValueCollection nameValueCollection;
        private ControllerContext controllerContext;
        private ModelBindingContext bindingContext;
        private TrimmingModelBinder binder;

        [SetUp]
        public void SetUp()
        {
            nameValueCollection = new NameValueCollection();
            controllerContext = new ControllerContext();
            bindingContext = new ModelBindingContext();
            binder = new TrimmingModelBinder();
            bindingContext.ModelName = "Text";
        }

        #region Method: BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)

        [Test]
        public void BindModel_ReturnNullThenThereIsNoValueProvider()
        {
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Assert.IsNull(binder.BindModel(controllerContext, bindingContext));
        }

        [Test]
        public void BindModel_ReturnNullThenModelPropertyValueIsNull()
        {
            nameValueCollection.Add("Text", null);
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Assert.IsNull(binder.BindModel(controllerContext, bindingContext));
        }

        [Test]
        public void BindModel_TrimsBindedModelsProperty()
        {
            nameValueCollection.Add("Text", "  Trimmed text  ");
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Object actual = binder.BindModel(controllerContext, bindingContext);
            Object expected = "Trimmed text";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
