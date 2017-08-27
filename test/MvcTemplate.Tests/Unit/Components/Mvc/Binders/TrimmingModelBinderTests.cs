using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class TrimmingModelBinderTests
    {
        private NameValueCollection collection;
        private ModelBindingContext binding;
        private TrimmingModelBinder binder;
        private ControllerContext context;

        public TrimmingModelBinderTests()
        {
            context = new ControllerContext();
            binding = new ModelBindingContext();
            binding.ModelName = "StringField";
            collection = new NameValueCollection();
            binder = new TrimmingModelBinder();

            context.Controller = Substitute.For<ControllerBase>();
            binding.ModelMetadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AllTypesView), "StringField");
        }

        #region BindModel(ControllerContext context, ModelBindingContext binding)

        [Fact]
        public void BindModel_NullValue_ReturnsNull()
        {
            binding.ValueProvider = new NameValueCollectionValueProvider(collection, null);

            Assert.Null(binder.BindModel(context, binding));
        }

        [Fact]
        public void BindModel_DoesNotTrimValue()
        {
            binding.ModelName = "NotTrimmedStringField";
            collection.Add(binding.ModelName, "  Trimmed text  ");
            binding.ValueProvider = new NameValueCollectionValueProvider(collection, null);
            binding.ModelMetadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AllTypesView), "NotTrimmedStringField");

            Object actual = binder.BindModel(context, binding);
            Object expected = "  Trimmed text  ";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BindModel_TrimsValue()
        {
            collection.Add(binding.ModelName, "  Trimmed text  ");
            binding.ValueProvider = new NameValueCollectionValueProvider(collection, null);

            Object actual = binder.BindModel(context, binding);
            Object expected = "Trimmed text";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
