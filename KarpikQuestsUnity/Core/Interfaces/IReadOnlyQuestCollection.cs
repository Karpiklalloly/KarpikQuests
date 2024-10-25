using NewKarpikQuests.Sample;

namespace NewKarpikQuests.Interfaces
{
    public interface IReadOnlyQuestCollection : IEnumerable<Quest>, IEquatable<IReadOnlyQuestCollection>
    {
        public bool Has(in Quest item);
    }
}