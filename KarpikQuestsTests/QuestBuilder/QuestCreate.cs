using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace Karpik.Quests.Tests.Builder;

public class QuestCreate
{
    private IQuestAggregator _aggregator;

    [SetUp]
    public void Setup()
    {
        _aggregator = new QuestAggregator();
    }
    
    [Test]
    public void WhenCreateQuest_AndSetName_ThenQuestHasThisName(
        [Values("name", "quest", "NAME")] string name)
    {
        var quest = QuestBuilder
            .Start<QuestSample.Quest>(name, "")
            .SetAggregator(_aggregator)
            .Build();

        Assert.That(quest.Name, Is.EqualTo(name));
    }

    [Test]
    public void WhenCreateQuest_AndSetDesc_ThenQuestHasThisDesc(
        [Values("desc", "quest", "DESC")] string desc)
    {
        var quest = QuestBuilder
            .Start<QuestSample.Quest>("", desc)
            .SetAggregator(_aggregator)
            .Build();
        
        Assert.That(quest.Description, Is.EqualTo(desc));
    }

    [Test]
    public void WhenCreateQuest_AndSetAndCompletionType_ThenQuestHasThisType()
    {
        ICompletionType type = new And();
        
        var quest = QuestBuilder
            .Start<QuestSample.Quest>("", "", null, type)
            .SetAggregator(_aggregator)
            .Build();
        
        Assert.That(quest.CompletionType, Is.EqualTo(type));
    }
    
    [Test]
    public void WhenCreateQuest_AndSetOrCompletionType_ThenQuestHasThisType()
    {
        ICompletionType type = new Or();
        
        var quest = QuestBuilder
            .Start<QuestSample.Quest>("", "", null, type)
            .SetAggregator(_aggregator)
            .Build();
        
        Assert.That(quest.CompletionType, Is.EqualTo(type));
    }
    
    [Test]
    public void WhenCreateQuest_AndSetNeededCountCompletionType_ThenQuestHasThisType()
    {
        ICompletionType type = new NeededCount(1);
        
        var quest = QuestBuilder
            .Start<QuestSample.Quest>("", "", null, type)
            .SetAggregator(_aggregator)
            .Build();
        
        Assert.That(quest.CompletionType, Is.EqualTo(type));
    }
}