using Karpik.Quests.Sample;

namespace Karpik.Quests.Interfaces
{
    public interface ICompletionType
    {
        public Status Check(IEnumerable<IRequirement> requirements);
    }
}