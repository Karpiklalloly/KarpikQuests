using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Extensions
{
    public static partial class BundleExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUnStarted(this ITaskBundle bundle)
        {
            return bundle.Status.IsUnStarted();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsStarted(this ITaskBundle bundle)
        {
            return bundle.Status.IsStarted();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompleted(this ITaskBundle bundle)
        {
            return bundle.Status.IsCompleted();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFailed(this ITaskBundle bundle)
        {
            return bundle.Status.IsFailed();
        }
    }
}