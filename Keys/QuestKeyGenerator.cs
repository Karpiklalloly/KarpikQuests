namespace KarpikQuests.Keys
{
    public static class QuestKeyGenerator
    {
        private static uint _autoUintKey = 0;

        public static string GenerateNextAutoKey()
        {
            return _autoUintKey++.ToString();
        }

        public static string[] GenerateNextAutoKeys(int count)
        {
            string[] keys = new string[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = GenerateNextAutoKey();
            }
            return keys;
        }
    }
}