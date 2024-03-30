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
    public void WhenBuilderCreatesQuest_AndSetName_ThenQuestsNameIsEquel([Values("name", "quest", "NAME")] string name)
    {
        string desc = "desc";

        var quest = QuestBuilder
            .Start<Quest>(name, desc, processor: null, completionType: null)
            .SetAggregator(_aggregator)
            .Create();

        Assert.That(quest.Name, Is.EqualTo(name));
    }

    [Test]
    public void WhenBuilderCreatesQuest_AndSetDesc_ThenQuestsDescIsEquel([Values("desc", "quest", "DESC")] string desc)
    {
        string name = "name";

        var quest = QuestBuilder
            .Start<Quest>(name, desc, processor: null, completionType: null)
            .SetAggregator(_aggregator)
            .Create();
        
        Assert.That(quest.Description, Is.EqualTo(desc));
    }

    [Test]
    public void WhenBuilderCreatesQuest_AndAddAggregatorOnCreate_ThenThereIsQuest()
    {
        string name = "name";
        string desc = "desc";

        QuestBuilder
            .Start<Quest>(name, desc, processor: null, completionType: null)
            .SetAggregator(_aggregator)
            .Create();

        Assert.That(_aggregator.Quests, Is.Not.Empty);
    }
}
