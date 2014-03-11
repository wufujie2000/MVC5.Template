using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.Tests.Helpers
{
    public static class TestHelper
    {
        private static Type StringType = typeof(String);

        public static void PropertyWiseEquals<T>(T expected, T actual)
        {
            if (Object.Equals(expected, actual)) return;
            if (expected == null || actual == null)
                throw new AssertionException(String.Format("{0} is not equal to {1}", expected, actual));
            
            var type = expected.GetType();
            if (type.IsValueType || type.IsEnum)
                throw new AssertionException(String.Format("{0} is not equal to {1}", expected, actual));

            if (type == StringType)
                if (expected as String != actual as String)
                    throw new AssertionException(String.Format("{0} is not equal to {1}", expected, actual));

            var validProperties = type
                .GetProperties()
                .Where(prop =>
                    prop.PropertyType.IsEnum ||
                    prop.PropertyType.IsValueType ||
                    prop.PropertyType == StringType);

            foreach (var property in validProperties)
                PropertyWiseEquals(property.GetValue(expected), property.GetValue(actual));
        }

        public static void EnumPropertyWiseEquals<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var expectedEnumerator = expected.GetEnumerator();
            var actualEnumerator = actual.GetEnumerator();

            while (expectedEnumerator.MoveNext() | actualEnumerator.MoveNext())
                PropertyWiseEquals(expectedEnumerator.Current, actualEnumerator.Current);
        }
    }
}
