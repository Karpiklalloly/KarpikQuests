using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Statuses;

namespace Karpik.Quests.Extensions
{
    public static partial class StatusExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompleted(this IStatus status)
        {
            return status is Completed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsStarted(this IStatus status)
        {
            return status is Started;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUnStarted(this IStatus status)
        {
            return status is UnStarted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFailed(this IStatus status)
        {
            return status is Failed;
        }
    }
}