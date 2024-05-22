using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;


namespace Karpik.Quests.Tests.Builder;

internal class QuestBuilderTests
{
    private IAggregator _aggregator;

    [SetUp]
    public void Setup()
    {
        _aggregator = new Aggregator();
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
