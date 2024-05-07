using Karpik.Quests.Factories;
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
        var quest1 = QuestFactory.Instance.Create("name1");
        var quest2 = QuestFactory.Instance.Create("name2");
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);

        //Condition
        aggregator.TryRemoveQuest(graph, quest1);

        //Result
        Assert.That(!aggregator.Has(quest1));
    }
}