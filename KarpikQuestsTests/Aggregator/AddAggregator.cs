using Karpik.Quests.Factories;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Aggregator;

public class AddAggregator
{
    [Test]
    public void WhenCreateAggregator_AndAddQuest_ThenAggregatorHasQuest()
    {
        //Action
        var aggregator = new Karpik.Quests.QuestSample.Aggregator();
        var graph = new Karpik.Quests.QuestSample.Graph();
        var quest = QuestFactory.Instance.Create("name");

        //Condition
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest);

        //Result
        Assert.That(aggregator.Has(quest));
    }
    
    [Test]
    public void WhenCreateAggregator_AndAddGraph_ThenAggregatorHasGraph()
    {
        //Action
        var aggregator = new Karpik.Quests.QuestSample.Aggregator();
        var graph = new Karpik.Quests.QuestSample.Graph();

        //Condition
        aggregator.TryAddGraph(graph);

        //Result
        Assert.That(aggregator.Has(graph));
    }
}