using Karpik.Quests;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Aggregator;

public class AddAggregator
{
    [Test]
    public void WhenCreateAggregator_AndAddQuest_ThenAggregatorHasQuest()
    {
        //Action
        var aggregator = new QuestAggregator();
        var graph = new QuestGraph();
        var quest = QuestBuilder.Start<Quest>("name", "desc")
            .Build();

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
        var aggregator = new QuestAggregator();
        var graph = new QuestGraph();

        //Condition
        aggregator.TryAddGraph(graph);

        //Result
        Assert.That(aggregator.Has(graph));
    }
}