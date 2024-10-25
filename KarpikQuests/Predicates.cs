using System.Runtime.CompilerServices;
using NewKarpikQuests.Extensions;
using NewKarpikQuests.Sample;

namespace NewKarpikQuests
{
    public static class Predicates
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool QuestIsCompleted(Quest quest)
    {
        return quest.IsCompleted();
    }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool QuestIsFailed(Quest quest)
    {
        return quest.IsFailed();
    }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool QuestIsUnlocked(Quest quest)
    {
        return quest.IsUnlocked();
    }
    }
}