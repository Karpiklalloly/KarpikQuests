using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Graph;

public class SetDependency
{
    [Test]
    public void WhenAddQuests_AndSetDependencies_ThenRightDependencies()
    {
        //Action
        IGraph graph = new QuestGraph();
        var node1 = new GraphNode(new Quest());
        var node2 = new GraphNode(new Quest());
        var node3 = new GraphNode(new Quest());
        graph.TryAdd(node1);
        graph.TryAdd(node2);
        graph.TryAdd(node3);

        //Condition
        graph.TrySetDependency(node1.NodeId, node2.NodeId, IGraph.DependencyType.Completion);
        graph.TrySetDependency(node1.NodeId, node3.NodeId, IGraph.DependencyType.Completion);

        //Result
        var dependencies = graph.GetDependenciesNodes(node1.NodeId).ToList();
        var d2 = graph.GetDependenciesNodes(node2.NodeId);
        var d3 = graph.GetDependenciesNodes(node3.NodeId);
        
        Assert.Multiple(() =>
        {
            Assert.That(dependencies.Find(x => x.Equals(node2)), Is.Not.EqualTo(null));
            Assert.That(dependencies.Find(x => x.Equals(node3)), Is.Not.EqualTo(null));
            Assert.That(!d2.Any());
            Assert.That(!d3.Any());
        });
    }
}