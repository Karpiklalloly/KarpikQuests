using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;


namespace Karpik.Quests.Tests.Builder;

internal class QuestBuilderTests
{
    private IQuestAggregator _aggregator;

    [SetUp]
    public void Setup()
    {
        _aggregator = new QuestAggregator();
    }
    
    [Test]
    public void WhenBuilderCreatesQuest_AndAddAggregatorOnCreate_ThenThereIsQuest()
    {
        QuestBuilder
            .Start<QuestSample.Quest>("", "")
            .SetAggregator(_aggregator)
            .Build();

        Assert.That(_aggregator.Quests, Is.Not.Empty);
    }
}
