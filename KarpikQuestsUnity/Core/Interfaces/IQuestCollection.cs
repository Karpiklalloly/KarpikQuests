using NewKarpikQuests.Sample;

namespace NewKarpikQuests.Interfaces
{
    public interface IQuestCollection : IReadOnlyQuestCollection, IList<Quest>
    {
        
    }
}