using Karpik.Quests.Sample;

namespace Karpik.Quests.Interfaces
{
    public interface IReadOnlyQuestCollection : IEnumerable<Quest>, IEquatable<IReadOnlyQuestCollection>
    {
        public bool Has(in Quest item);
    }
}