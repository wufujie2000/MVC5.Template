using System;

namespace Template.Components.Security
{
    public static class BCrypter
    {
        private const Int32 WorkFactor = 6;
        private const Int32 PasswordWorkFactor = 13;

        public static String Hash(String value)
        {
            return BCrypt.Net.BCrypt.HashString(value, WorkFactor);
        }
        public static String HashPassword(String value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value, PasswordWorkFactor);
        }

        public static Boolean Verify(String value, String hash)
        {
            return BCrypt.Net.BCrypt.Verify(value, hash);
        }
    }
}
