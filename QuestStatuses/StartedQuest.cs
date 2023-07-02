using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;

namespace KarpikQuests.QuestStatuses
{
    public class StartedQuest : QuestStatusBase
    {
        public override bool Equals(IQuestStatus other)
        {
            if (other is StartedQuest)
            {
                return true;
            }
            return false;
        }
    }
}