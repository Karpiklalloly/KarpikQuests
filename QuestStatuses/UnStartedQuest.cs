using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
using System;

namespace KarpikQuests.QuestStatuses
{
    [Serializable]
    public class UnStartedQuest : QuestStatusBase
    {
        public override bool Equals(IQuestStatus other)
        {
            if (other is UnStartedQuest)
            {
                return true;
            }
            return false;
        }
    }
}