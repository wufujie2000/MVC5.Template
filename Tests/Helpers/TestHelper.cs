using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.Tests.Helpers
{
    public static class TestHelper
    {
        private static Type String = typeof(String);

        public static Boolean PropertyWiseEquals<T>(T expected, T actual)
        {
            if (Object.Equals(expected, actual)) return true;
            if (expected == null || actual == null) return false;

            var type = expected.GetType();
            if (type.IsValueType || type == String)
                return false;

            var validProperties = type
                .GetProperties()
                .Where(prop =>
                    prop.PropertyType.IsValueType ||
                    prop.PropertyType == String);

            foreach (var property in validProperties)
            {
                var obj1Value = property.GetValue(expected);
                var obj2Value = property.GetValue(actual);
                if (!Object.Equals(expected, actual))
                    return false;
            }

            return true;
        }

        public static Boolean PropertyWiseEquals<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var expectedEnumerator = expected.GetEnumerator();
            var actualEnumerator = actual.GetEnumerator();

            while (expectedEnumerator.MoveNext() | actualEnumerator.MoveNext())
                if (!PropertyWiseEquals(expectedEnumerator.Current, actualEnumerator.Current))
                    return false;

            return true;
        }
    }
}
