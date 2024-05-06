using Karpik.Quests;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Aggregator;

public class GetGraph
{
    [Test]
    public void WhenAddQuest_AndGetIt_ThenItIsGain()
    {
        //Action
        var aggregator = new QuestAggregator();
        var graph = new QuestGraph();
        var quest = QuestBuilder.Start<Quest>("name", "desc")
            .Build();
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest);

        //Condition
        var quest2 = aggregator.Get(quest.Id);

        //Result
        Assert.That(quest, Is.EqualTo(quest2));
    }
}