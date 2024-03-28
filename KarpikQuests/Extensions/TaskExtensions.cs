using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Extensions
{
    public static class TaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUnStarted(this ITask task)
        {
            return task.Status == ITask.TaskStatus.UnStarted;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsStarted(this ITask task)
        {
            return task.Status == ITask.TaskStatus.Started;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompleted(this ITask task)
        {
            return task.Status == ITask.TaskStatus.Completed;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFailed(this ITask task)
        {
            return task.Status == ITask.TaskStatus.Failed;
        }
    }
}
