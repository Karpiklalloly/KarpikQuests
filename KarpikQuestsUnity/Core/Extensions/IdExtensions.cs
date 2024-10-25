using System.Runtime.CompilerServices;
using NewKarpikQuests.ID;

namespace NewKarpikQuests.Extensions
{
    public static class IdExtensions
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