using System;

namespace KarpikQuests.Interfaces
{
    public interface IQuestStatus : IEquatable<IQuestStatus>
    {
        public string GetStatus();
    }
}