using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;

namespace KarpikQuests.QuestStatuses
{
    public class CompletedQuest : QuestStatusBase
    {
        public override bool Equals(IQuestStatus other)
        {
            if (other is CompletedQuest)
            {
                return true;
            }
            return false;
        }
    }
}