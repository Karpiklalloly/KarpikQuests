using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.QuestStatuses
{
    public class StartedQuest : IQuestStatus
    {
        public bool Equals(IQuestStatus other)
        {
            if (other is StartedQuest)
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