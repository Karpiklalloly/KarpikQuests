namespace KarpikQuests.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValid(this string str)
        {
            if (str.IsNullOrEmpty()) return false;
            if (str.IsNullOrWhiteSpaces()) return false;

            return true;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpaces(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
