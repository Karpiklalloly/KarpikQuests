using System.Runtime.CompilerServices;

namespace Karpik.Quests.Extensions
{
    public static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this string str)
        {
            if (str.IsNullOrEmpty()) return false;
            if (str.IsNullOrWhiteSpaces()) return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrWhiteSpaces(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
