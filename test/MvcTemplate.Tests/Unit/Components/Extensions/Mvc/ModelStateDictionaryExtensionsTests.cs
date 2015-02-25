using MvcTemplate.Components.Extensions.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Mvc
{
    public class ModelStateDictionaryExtensionsTests
    {
        Expression<Func<ModelStateView, Object>> expression;
        private ModelStateDictionary modelState;

        public ModelStateDictionaryExtensionsTests()
        {
            expression = (model) => model.Relation.Id;
            modelState = new ModelStateDictionary();
        }

        #region Extension method: AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, Exception exception)

        [Fact]
        public void AddModelError_AddsModelExceptionKey()
        {
            modelState.AddModelError(expression, new Exception());

            String expected = ExpressionHelper.GetExpressionText(expression);
            String actual = modelState.Single().Key;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddModelError_AddsModelException()
        {
            Exception exception = new Exception();
            modelState.AddModelError(expression, exception);

            Exception actual = modelState.Single().Value.Errors.Single().Exception;
            Exception expected = exception;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Extension method: AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, String errorMessage)

        [Fact]
        public void AddModelError_AddsModelErrorKey()
        {
            modelState.AddModelError(expression, "Test error");

            String expected = ExpressionHelper.GetExpressionText(expression);
            String actual = modelState.Single().Key;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddModelError_AddsModelErrorMessage()
        {
            modelState.AddModelError(expression, "Test error");

            String actual = modelState.Single().Value.Errors.Single().ErrorMessage;
            String expected = "Test error";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, String format, Object[] args)

        [Fact]
        public void AddModelError_Format_AddsModelErrorKey()
        {
            modelState.AddModelError(expression, "Test {0}", "error");

            String expected = ExpressionHelper.GetExpressionText(expression);
            String actual = modelState.Single().Key;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddModelError_Format_AddsFormattedModelErrorMessage()
        {
            modelState.AddModelError(expression, "Test {0}", "error");

            String actual = modelState.Single().Value.Errors.Single().ErrorMessage;
            String expected = "Test error";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
