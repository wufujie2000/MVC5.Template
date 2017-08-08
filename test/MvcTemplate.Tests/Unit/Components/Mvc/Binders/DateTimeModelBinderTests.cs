using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DateTimeModelBinderTests
    {
        private ControllerContext controllerContext;
        private ModelBindingContext bindingContext;
        private NameValueCollection collection;
        private DateTimeModelBinder binder;

        public DateTimeModelBinderTests()
        {
            controllerContext = new ControllerContext();
            bindingContext = new ModelBindingContext();
            bindingContext.ModelName = "DateTimeField";
            collection = new NameValueCollection();
            binder = new DateTimeModelBinder();

            controllerContext.Controller = Substitute.For<ControllerBase>();
            bindingContext.ModelMetadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AllTypesView), "DateTimeField");
        }

        #region BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)

        [Fact]
        public void BindModel_NullValue_ReturnsNull()
        {
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(collection, null);

            Assert.Null(binder.BindModel(controllerContext, bindingContext));
        }

        [Fact]
        public void BindModel_DoesNotTruncateValue()
        {
            collection.Add(bindingContext.ModelName, new DateTime(2017, 2, 3, 4, 5, 6).ToString());
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(collection, null);

            Object actual = binder.BindModel(controllerContext, bindingContext);
            Object expected = new DateTime(2017, 2, 3, 4, 5, 6);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BindModel_TruncatesValue()
        {
            bindingContext.ModelName = "TruncatedDateTimeField";
            collection.Add(bindingContext.ModelName, new DateTime(2017, 2, 3, 4, 5, 6).ToString());
            bindingContext.ValueProvider = new NameValueCollectionValueProvider(collection, null);
            bindingContext.ModelMetadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AllTypesView), "TruncatedDateTimeField");

            Object actual = binder.BindModel(controllerContext, bindingContext);
            Object expected = new DateTime(2017, 2, 3);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
