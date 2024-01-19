using System;

namespace KarpikQuests.Keys
{
    public static class KeyGenerator
    {
        private const string _defaultPrefix = "";

        public static string GenerateKey(string prefix = _defaultPrefix)
        {
            string key = Guid.NewGuid().ToString();
            if (prefix == _defaultPrefix)
            {
                return key;
            }
            return prefix + key;
        }

        public static Guid GenerateKey()
        {
            return Guid.NewGuid();
        }

        public static string[] GenerateKeys(int count, string prefix = _defaultPrefix)
        {
            string[] keys = new string[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = GenerateKey(prefix);
            }
            return keys;
        }

        public static Guid[] GenerateKeys(int count)
        {
            Guid[] keys = new Guid[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = GenerateKey();
            }
            return keys;
        }
    }
}