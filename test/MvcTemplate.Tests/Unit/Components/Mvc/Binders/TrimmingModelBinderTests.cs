using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
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
            bindingContext.ModelName = "Trimmed";
            binder = new TrimmingModelBinder();

            bindingContext.ModelMetadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(BindersModel), bindingContext.ModelName);
        }

        #region Method: BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)

        [Test]
        public void BindModel_ReturnNullThenThereIsNoValueProvider()
        {
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Assert.IsNull(binder.BindModel(controllerContext, bindingContext));
        }

        [Test]
        public void BindModel_OnNullValueReturnsNull()
        {
            nameValueCollection.Add(bindingContext.ModelName, null);
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Assert.IsNull(binder.BindModel(controllerContext, bindingContext));
        }

        [Test]
        public void BindModel_DoNotTrimPropertyWithNotTrimmedAttribute()
        {
            bindingContext.ModelName = "NotTrimmed";
            nameValueCollection.Add(bindingContext.ModelName, "  Trimmed text  ");
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);
            bindingContext.ModelMetadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(BindersModel), "NotTrimmed");

            Object actual = binder.BindModel(controllerContext, bindingContext);
            Object expected = "  Trimmed text  ";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BindModel_TrimsBindedModelsProperty()
        {
            nameValueCollection.Add(bindingContext.ModelName, "  Trimmed text  ");
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            Object actual = binder.BindModel(controllerContext, bindingContext);
            Object expected = "Trimmed text";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
