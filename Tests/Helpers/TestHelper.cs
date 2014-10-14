using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Tests.Helpers
{
    public static class TestHelper
    {
        private static Type StringType = typeof(String);

        public static void PropertyWiseEqual<T>(T expected, T actual)
        {
            if (Equals(expected, actual)) return;
            if (expected == null || actual == null)
                throw new AssertionException(String.Format("'{0}' is not equal to '{1}'.", expected, actual));

            Type type = expected.GetType();
            if (type.IsValueType || type.IsEnum)
                throw new AssertionException(String.Format("'{0}' is not equal to '{1}'.", expected, actual));

            if (type == StringType)
                if (expected as String != actual as String)
                    throw new AssertionException(String.Format("'{0}' is not equal to '{1}'.", expected, actual));

            IEnumerable<PropertyInfo> validProperties = type
                .GetProperties()
                .Where(prop =>
                    prop.PropertyType.IsEnum ||
                    prop.PropertyType.IsValueType ||
                    prop.PropertyType == StringType);

            foreach (PropertyInfo property in validProperties)
                PropertyWiseEqual(property.GetValue(expected), property.GetValue(actual));
        }
        public static void EnumPropertyWiseEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            IEnumerator<T> expectedEnumerator = expected.GetEnumerator();
            IEnumerator<T> actualEnumerator = actual.GetEnumerator();

            while (expectedEnumerator.MoveNext() | actualEnumerator.MoveNext())
                PropertyWiseEqual(expectedEnumerator.Current, actualEnumerator.Current);
        }

        public static void EnumeratorsEqual(IEnumerator expected, IEnumerator actual)
        {
            while (expected.MoveNext() | actual.MoveNext())
                Assert.AreSame(expected.Current, actual.Current);
        }
    }
}
