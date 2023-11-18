using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
using System;

namespace KarpikQuests.QuestStatuses
{
    [Serializable]
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