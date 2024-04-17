using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Graph;

public class IsCyclic
{
    [Test]
    public void WhenCreateCyclic_AndCallIsCyclic_ThenTrue()
    {
        //Action
        IGraph graph = new QuestGraph();
        var node1 = new GraphNode(new Quest());
        var node2 = new GraphNode(new Quest());
        var node3 = new GraphNode(new Quest());
        graph.TryAdd(node1);
        graph.TryAdd(node2);
        graph.TryAdd(node3);
        graph.TrySetDependency(node1.NodeId, node2.NodeId, IGraph.DependencyType.Completion);
        graph.TrySetDependency(node2.NodeId, node3.NodeId, IGraph.DependencyType.Completion);
        graph.TrySetDependency(node3.NodeId, node1.NodeId, IGraph.DependencyType.Completion);

        //Condition
        var result = graph.IsCyclic();

        //Result
        Assert.That(result);
    }
    
    [Test]
    public void WhenCreateCyclic_AndCallIsCyclic_ThenTrue2()
    {
        //Action
        IGraph graph = new QuestGraph();
        var node1 = new GraphNode(new Quest());
        var node2 = new GraphNode(new Quest());
        var node3 = new GraphNode(new Quest());
        var node4 = new GraphNode(new Quest());
        graph.TryAdd(node1);
        graph.TryAdd(node2);
        graph.TryAdd(node3);
        graph.TryAdd(node4);
        graph.TrySetDependency(node2.NodeId, node1.NodeId, IGraph.DependencyType.Completion);
        graph.TrySetDependency(node2.NodeId, node3.NodeId, IGraph.DependencyType.Completion);
        graph.TrySetDependency(node3.NodeId, node4.NodeId, IGraph.DependencyType.Completion);
        graph.TrySetDependency(node4.NodeId, node2.NodeId, IGraph.DependencyType.Completion);

        //Condition
        var result = graph.IsCyclic();

        //Result
        Assert.That(result);
    }
    
    [Test]
    public void WhenCreateCyclic_AndCallIsCyclic_ThenTrue3()
    {
        //Action
        IGraph graph = new QuestGraph();
        var node1 = new GraphNode(new Quest());
        var node2 = new GraphNode(new Quest());
        var node3 = new GraphNode(new Quest());
        var node4 = new GraphNode(new Quest());
        graph.TryAdd(node1);
        graph.TryAdd(node2);
        graph.TryAdd(node3);
        graph.TryAdd(node4);
        graph.TrySetDependency(node2.NodeId, node3.NodeId, IGraph.DependencyType.Completion);
        graph.TrySetDependency(node3.NodeId, node4.NodeId, IGraph.DependencyType.Completion);
        graph.TrySetDependency(node4.NodeId, node2.NodeId, IGraph.DependencyType.Completion);

        //Condition
        var result = graph.IsCyclic();

        //Result
        Assert.That(result);
    }
}