using NUnit.Framework;
using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using Template.Components.Mvc.Binders;

namespace Template.Tests.Unit.Components.Mvc.Binders
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

            Object actual = binder.BindModel(controllerContext, bindingContext);

            Assert.IsNull(actual);
        }

        [Test]
        public void BindModel_ReturnNullThenModelPropertyValueIsNull()
        {
            nameValueCollection.Add("Text", null);
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Object actual = binder.BindModel(controllerContext, bindingContext);

            Assert.IsNull(actual);
        }

        [Test]
        public void BindModel_TrimsBindedModelsProperty()
        {
            nameValueCollection.Add("Text", "  Trimmed text  ");
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            String expected = "Trimmed text";
            Object actual = binder.BindModel(controllerContext, bindingContext);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
