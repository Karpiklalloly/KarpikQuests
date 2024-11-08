using System.Collections.Generic;

namespace Karpik.Quests
{
    public interface IQuestCollection : IReadOnlyQuestCollection, IList<Quest>
    {
        
    }
}