using KarpikQuests.Interfaces;

namespace KarpikQuests.Extensions
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
