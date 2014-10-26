using MvcTemplate.Components.Extensions.Mvc;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Mvc
{
    [TestFixture]
    public class ModelStateDictionaryExtensionsTests
    {
        Expression<Func<ModelStateView, Object>> expression;
        private ModelStateDictionary modelState;

        [SetUp]
        public void Setup()
        {
            expression = (model) => model.Relation.Id;
            modelState = new ModelStateDictionary();
        }

        #region Extension method: AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, String errorMessage)

        [Test]
        public void AddModelError_AddsModelErrorKey()
        {
            modelState.AddModelError(expression, "Test error");

            String expected = ExpressionHelper.GetExpressionText(expression);
            String actual = modelState.Single().Key;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddModelError_AddsModelErrorMessage()
        {
            modelState.AddModelError(expression, "Test error");

            String actual = modelState.Single().Value.Errors.Single().ErrorMessage;
            String expected = "Test error";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, Exception exception)

        [Test]
        public void AddModelError_AddsModelExceptionKey()
        {
            modelState.AddModelError(expression, new Exception());

            String expected = ExpressionHelper.GetExpressionText(expression);
            String actual = modelState.Single().Key;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddModelError_AddsModelException()
        {
            Exception expected = new Exception();
            modelState.AddModelError(expression, expected);
            Exception actual = modelState.Single().Value.Errors.Single().Exception;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
