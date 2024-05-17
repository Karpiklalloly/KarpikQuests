using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface IQuestCollection : IReadOnlyQuestCollection, IList<IQuest>
    {
        
    }
}