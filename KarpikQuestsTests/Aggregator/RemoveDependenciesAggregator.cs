using Karpik.Quests.DependencyTypes;
using Karpik.Quests.Factories;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Aggregator;

public class RemoveDependenciesAggregator
{
    [Test]
    public void WhenSetDependencies_AndRemoveDependencies_ThenNoDependencies()
    {
        //Action
        var aggregator = new QuestAggregator();
        var graph = new QuestGraph();
        var quest1 = QuestFactory.Instance.Create("name1");
        var quest2 = QuestFactory.Instance.Create("name2");
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);
        aggregator.TryAddDependence(graph, quest2, quest1, new Completion());

        //Condition
        aggregator.TryRemoveDependencies(graph, quest2);
        
        var dependencies = aggregator.GetDependencies(graph, quest2);
        //Result
        Assert.That(dependencies, Has.Count.EqualTo(0));
    }
    
    [Test]
    public void WhenSetDependencies_AndRemoveDependencies_ThenNoDependents()
    {
        //Action
        var aggregator = new QuestAggregator();
        var graph = new QuestGraph();
        var quest1 = QuestFactory.Instance.Create("name1");
        var quest2 = QuestFactory.Instance.Create("name2");
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);
        aggregator.TryAddDependence(graph, quest2, quest1, new Completion());

        //Condition
        aggregator.TryRemoveDependencies(graph, quest2);
        
        var dependents = aggregator.GetDependents(graph, quest1);
        //Result
        Assert.That(dependents, Has.Count.EqualTo(0));
    }
    
    [Test]
    public void WhenSetDependency_AndRemoveDependency_ThenNoDependencies()
    {
        //Action
        var aggregator = new QuestAggregator();
        var graph = new QuestGraph();
        var quest1 = QuestFactory.Instance.Create("name1");
        var quest2 = QuestFactory.Instance.Create("name2");
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);
        aggregator.TryAddDependence(graph, quest2, quest1, new Completion());

        //Condition
        aggregator.TryRemoveDependence(graph, quest2, quest1);
        
        var dependencies = aggregator.GetDependencies(graph, quest2);
        //Result
        Assert.That(dependencies, Has.Count.EqualTo(0));
    }
    
    [Test]
    public void WhenSetDependency_AndRemoveDependency_ThenNoDependents()
    {
        //Action
        var aggregator = new QuestAggregator();
        var graph = new QuestGraph();
        var quest1 = QuestFactory.Instance.Create("name1");
        var quest2 = QuestFactory.Instance.Create("name2");
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);
        aggregator.TryAddDependence(graph, quest2, quest1, new Completion());

        //Condition
        aggregator.TryRemoveDependence(graph, quest2, quest1);
        
        var dependents = aggregator.GetDependents(graph, quest1);
        //Result
        Assert.That(dependents, Has.Count.EqualTo(0));
    }
}