using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.QuestStatuses
{
    //TODO: Add base class
    public class CompletedQuest : IQuestStatus
    {
        public bool Equals(IQuestStatus other)
        {
            if (other is CompletedQuest)
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