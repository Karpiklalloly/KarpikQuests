using System;

namespace KarpikQuests.Keys
{
    public static class KeyGenerator
    {
        private const string DefaultPrefix = "";

        public static string GenerateKey(string prefix = DefaultPrefix)
        {
            var key = Guid.NewGuid().ToString();
            if (prefix == DefaultPrefix)
            {
                return key;
            }
            return prefix + key;
        }

        public static Guid GenerateKeyGuid()
        {
            return Guid.NewGuid();
        }

        public static string[] GenerateKeys(int count, string prefix = DefaultPrefix)
        {
            var keys = new string[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = GenerateKey(prefix);
            }
            return keys;
        }

        public static Guid[] GenerateKeysGuid(int count)
        {
            var keys = new Guid[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = GenerateKeyGuid();
            }
            return keys;
        }
    }
}