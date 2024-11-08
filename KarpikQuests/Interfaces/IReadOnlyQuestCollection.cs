namespace Karpik.Quests
{
    public interface IReadOnlyQuestCollection : IEnumerable<Quest>, IEquatable<IReadOnlyQuestCollection>
    {
        public bool Has(in Quest item);
    }
}