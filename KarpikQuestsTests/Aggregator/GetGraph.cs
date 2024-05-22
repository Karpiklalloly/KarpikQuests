using Karpik.Quests;
using Karpik.Quests.Factories;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Aggregator;

public class GetGraph
{
    [Test]
    public void WhenAddQuest_AndGetIt_ThenItIsGain()
    {
        //Action
        var aggregator = new Karpik.Quests.QuestSample.Aggregator();
        var graph = new Karpik.Quests.QuestSample.Graph();
        var quest = QuestFactory.Instance.Create("name1");
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest);

        //Condition
        var quest2 = aggregator.Get(quest.Id);

        //Result
        Assert.That(quest, Is.EqualTo(quest2));
    }
}