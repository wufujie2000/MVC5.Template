using System;

namespace Template.Components.Security
{
    public class BCrypter : IHasher
    {
        private const Int32 WorkFactor = 6;
        private const Int32 PasswordWorkFactor = 13;

        public String Hash(String value)
        {
            return BCrypt.Net.BCrypt.HashString(value, WorkFactor);
        }
        public String HashPassword(String value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value, PasswordWorkFactor);
        }

        public Boolean Verify(String value, String hash)
        {
            return BCrypt.Net.BCrypt.Verify(value, hash);
        }
    }
}
