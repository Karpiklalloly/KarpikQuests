using System;
using System.Collections.Generic;
using Karpik.Quests.QuestSample;

namespace Karpik.Quests.Interfaces
{
    public interface IReadOnlyQuestCollection : IEnumerable<IQuest>, IEquatable<IReadOnlyQuestCollection>
    {
        public bool Has(IQuest item);
    }
}
