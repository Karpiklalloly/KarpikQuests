using System.Runtime.CompilerServices;
using Karpik.Quests.ID;

namespace Karpik.Quests.Extensions
{
    public static partial class IdExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this Id id)
        {
            return !id.IsEmpty();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Id id)
        {
            return id.Equals(Id.Empty);
        }
    }
}