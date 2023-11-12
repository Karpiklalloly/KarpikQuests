using System;

namespace KarpikQuests.Keys
{
    public static class QuestKeyGenerator
    {
        private const string _defaultPrefix = "";

        public static string GenerateNextAutoKey(string prefix = _defaultPrefix)
        {
            string key = Guid.NewGuid().ToString();
            if (prefix == _defaultPrefix)
            {
                return key;
            }
            return prefix + key;
        }

        public static Guid GenerateNextAutoKey()
        {
            return Guid.NewGuid();
        }

        public static string[] GenerateNextAutoKeys(int count, string prefix = _defaultPrefix)
        {
            string[] keys = new string[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = GenerateNextAutoKey(prefix);
            }
            return keys;
        }

        public static Guid[] GenerateNextAutoKeys(int count)
        {
            Guid[] keys = new Guid[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = GenerateNextAutoKey();
            }
            return keys;
        }
    }
}