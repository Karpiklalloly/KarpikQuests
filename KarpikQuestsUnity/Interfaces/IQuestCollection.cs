using Karpik.Quests.Sample;

namespace Karpik.Quests.Interfaces
{
    public interface IQuestCollection : IReadOnlyQuestCollection, IList<Quest>
    {
        
    }
}