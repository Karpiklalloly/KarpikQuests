using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
using System;

namespace KarpikQuests.QuestStatuses
{
    [Serializable]
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