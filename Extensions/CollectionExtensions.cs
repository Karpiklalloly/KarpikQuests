using System.Collections.Generic;

namespace KarpikQuests.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsValid<T>(this ICollection<T>? collection)
        {
            return collection != null;
        }
    }
}