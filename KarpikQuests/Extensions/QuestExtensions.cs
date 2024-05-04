using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Statuses;

namespace Karpik.Quests.Extensions
{
    public static partial class QuestExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompleted(this IQuest quest)
        {
            return quest.Status is Completed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsStarted(this IQuest quest)
        {
            return quest.Status is Started;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUnStarted(this IQuest quest)
        {
            return quest.Status is UnStarted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFailed(this IQuest quest)
        {
            return quest.Status is Failed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinished(this IQuest quest)
        {
            return quest.IsCompleted() || quest.IsFailed();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this IQuest quest)
        {
            if (quest is null) return false;
            
            return quest.Id.IsValid();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnStart(this IQuest quest)
        {
            foreach (var bundle in quest.TaskBundles)
            {
                bundle.Reset();
            }
            quest.SetStatus(new UnStarted());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Start(this IQuest quest)
        {
            quest.TaskBundles.Setup(quest.Processor);
            quest.SetStatus(new Started());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fail(this IQuest quest)
        {
            quest.SetStatus(new Failed());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Complete(this IQuest quest)
        {
            quest.SetStatus(new Completed());
        }
    }
}
