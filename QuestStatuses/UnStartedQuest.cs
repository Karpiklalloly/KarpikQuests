using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.QuestStatuses
{
    public class UnStartedQuest : IQuestStatus
    {
        public bool Equals(IQuestStatus other)
        {
            if (other is UnStartedQuest)
            {
                return true;
            }
            return false;
        }

        public string GetStatus()
        {
            throw new NotImplementedException();
        }
    }
}