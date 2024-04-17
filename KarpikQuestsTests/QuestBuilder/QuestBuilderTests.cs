using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;


namespace Karpik.Quests.Tests;

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
            .Start<Quest>("", "")
            .SetAggregator(_aggregator)
            .Create();

        Assert.That(_aggregator.Quests, Is.Not.Empty);
    }
}
