using NewKarpikQuests.Sample;

namespace NewKarpikQuests.Interfaces
{
    public interface ICompletionType
    {
        public Status Check(IEnumerable<Quest> quests);
    }
}