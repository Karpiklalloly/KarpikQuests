using System;
using System.Collections.Generic;
using System.Linq;

namespace Karpik.Quests.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Has<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.Any(predicate);
        }

        public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            int i = 0;
            foreach (var item1 in collection)
            {
                if (predicate(item1)) return i;
                i++;
            }
        
            return -1;
        }
    }
}