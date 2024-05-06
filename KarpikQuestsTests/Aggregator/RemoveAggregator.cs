using Karpik.Quests;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Aggregator;

public class RemoveAggregator
{
    [Test]
    public void WhenAddQuest_AndRemoveQuest_ThenNoThisQuest()
    {
        //Action
        var aggregator = new QuestAggregator();
        var graph = new QuestGraph();
        var quest1 = QuestBuilder.Start<Quest>("name1", "desc1")
            .Build();
        var quest2 = QuestBuilder.Start<Quest>("name2", "desc2")
            .Build();
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);

        //Condition
        aggregator.TryRemoveQuest(graph, quest1);

        //Result
        Assert.That(!aggregator.Has(quest1));
    }
}