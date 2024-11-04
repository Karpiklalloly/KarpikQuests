namespace Karpik.Quests.ID
{
    public static class IDGenerator
    {
        public const string DefaultPrefix = "";

        public static Id GenerateId(string prefix = DefaultPrefix)
        {
            var key = GenerateIdGuid().ToString();
            return new Id(prefix + key);
        }

        public static Guid GenerateIdGuid()
        {
            return Guid.NewGuid();
        }

        public static Id[] GenerateIds(int count, string prefix = DefaultPrefix)
        {
            var keys = new Id[count];
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