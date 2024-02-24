using KarpikQuests.Interfaces;

namespace KarpikQuests.Extensions
{
    public static class TaskExtensions
    {
        public static bool IsCompleted(this ITask task)
        {
            return task.Status == ITask.TaskStatus.Completed;
        }

        public static bool IsUnCompleted(this ITask task)
        {
            return task.Status == ITask.TaskStatus.UnCompleted;
        }
    }
}
