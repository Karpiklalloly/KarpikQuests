using Karpik.Quests.DependencyTypes;
using Karpik.Quests.Factories;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Aggregator;

public class GetDependenciesAggregator
{
    [Test]
    public void WhenSetDependencies_AndGetDependencies_ThenCorrectDependencies()
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
        var dependencies = aggregator.GetDependencies(graph, quest2);

        //Result
        Assert.That(dependencies, Has.Count.EqualTo(1));
        Assert.That(dependencies.Has(quest1));
    }
    
    [Test]
    public void WhenSetDependencies_AndGetDependents_ThenCorrectDependents()
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
        var dependents = aggregator.GetDependents(graph, quest1);

        //Result
        Assert.That(dependents, Has.Count.EqualTo(1));
        Assert.That(dependents.Has(quest2));
    }
}