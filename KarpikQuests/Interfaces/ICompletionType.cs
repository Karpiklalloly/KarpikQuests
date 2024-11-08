namespace Karpik.Quests
{
    public interface ICompletionType
    {
        public Status Check(IEnumerable<IRequirement> requirements);
    }
}