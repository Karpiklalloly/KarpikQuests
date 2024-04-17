using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Graph;

public class GetDependents
{
    [Test]
    public void WhenAddDependents_AndGetDependents_ThenQuestHasTheeseDependets()
    {
        //Action
        IGraph graph = new QuestGraph();
        var node1 = new GraphNode(new Quest());
        var node2 = new GraphNode(new Quest());
        var node3 = new GraphNode(new Quest());
        graph.TryAdd(node1);
        graph.TryAdd(node2);
        graph.TryAdd(node3);
        graph.TrySetDependency(node2.NodeId, node1.NodeId, IGraph.DependencyType.Completion);
        graph.TrySetDependency(node3.NodeId, node1.NodeId, IGraph.DependencyType.Completion);

        //Condition
        var dependents = graph.GetDependentsNodes(node1.NodeId).ToList();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(dependents.Find(x => x.Equals(node2)), Is.Not.EqualTo(null));
            Assert.That(dependents.Find(x => x.Equals(node3)), Is.Not.EqualTo(null));
        });
    }
}