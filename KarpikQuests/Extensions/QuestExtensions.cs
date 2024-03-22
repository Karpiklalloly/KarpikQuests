using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Extensions
{
    public static class QuestExtensions
    {
        public static bool IsValid(this IQuest? quest)
        {
            if (quest is null) return false;
            
            return quest.Key.IsValid();
        }
    }
}
