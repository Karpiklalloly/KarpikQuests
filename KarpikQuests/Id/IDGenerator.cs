using System;

namespace Karpik.Quests.ID
{
    public static class IDGenerator
    {
        public const string DefaultPrefix = "";

        public static string GenerateId(string prefix = DefaultPrefix)
        {
            var key = GenerateIdGuid().ToString();
            if (prefix == DefaultPrefix)
            {
                return key;
            }
            return prefix + key;
        }

        public static Guid GenerateIdGuid()
        {
            return Guid.NewGuid();
        }

        public static string[] GenerateIds(int count, string prefix = DefaultPrefix)
        {
            var keys = new string[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = GenerateId(prefix);
            }
            return keys;
        }

        public static Guid[] GenerateIdsGuid(int count)
        {
            var keys = new Guid[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = GenerateIdGuid();
            }
            return keys;
        }
    }
}