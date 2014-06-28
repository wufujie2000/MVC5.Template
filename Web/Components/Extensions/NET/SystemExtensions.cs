using System;

namespace MvcTemplate.Components.Extensions.Net
{
    public static class SystemExtensions
    {
        #region Int32

        public static Int32 LimitTo(this Int32 number, Int32 lowerBound, Int32 upperBound)
        {
            number = number > lowerBound ? number : lowerBound;
            number = number < upperBound ? number : upperBound;

            return number;
        }

        #endregion

        #region String

        public static String[] Split(this String str, String separator)
        {
            return str.Split(separator, StringSplitOptions.None);
        }
        public static String[] Split(this String str, Char separator, Int32 count)
        {
            return str.Split(new Char[] { separator }, count);
        }
        public static String[] Split(this String str, String separator, Int32 count)
        {
            return str.Split(separator, count, StringSplitOptions.None);
        }
        public static String[] Split(this String str, Char separator, StringSplitOptions options)
        {
            return str.Split(new Char[] { separator }, options);
        }
        public static String[] Split(this String str, String separator, StringSplitOptions options)
        {
            return str.Split(new String[] { separator }, options);
        }
        public static String[] Split(this String str, Char separator, Int32 count, StringSplitOptions options)
        {
            return str.Split(new Char[] { separator }, count, options);
        }
        public static String[] Split(this String str, String separator, Int32 count, StringSplitOptions options)
        {
            return str.Split(new String[] { separator }, count, options);
        }

        #endregion
    }
}
