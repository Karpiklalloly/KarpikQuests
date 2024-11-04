namespace Karpik.Quests.Interfaces;

public interface IRequirement
{
    public bool IsSatisfied();
    public bool IsRuined();
    public void SetGraph(IGraph graph);
}