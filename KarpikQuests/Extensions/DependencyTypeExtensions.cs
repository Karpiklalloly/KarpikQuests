using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Extensions
{
    public static partial class DependencyTypeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this IDependencyType type)
        {
            if (type is null) return false;
        
            return true;
        }
    }
}