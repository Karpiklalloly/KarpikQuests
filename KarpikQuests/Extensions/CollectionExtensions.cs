using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Karpik.Quests.Extensions
{
    public static partial class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid<T>(this ICollection<T>? collection)
        {
            return collection != null;
        }
    }
}