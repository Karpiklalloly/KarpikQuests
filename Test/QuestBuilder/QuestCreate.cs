using NewKarpikQuests;
using NewKarpikQuests.CompletionTypes;
using NewKarpikQuests.Interfaces;
using NewKarpikQuests.Processors;

namespace KarpikQuestsTests.BuilderTests
{
    public class QuestCreate
    {
        [Test]
        public void WhenCreateQuest_AndSetName_ThenQuestHasThisName(
            [Values("name", "quest", "NAME")] string name)
    {
        var quest = QuestBuilder
            .Start(name, "")
            .Build();

        Assert.That(quest.Name, Is.EqualTo(name));
    }

        [Test]
        public void WhenCreateQuest_AndSetDesc_ThenQuestHasThisDesc(
            [Values("desc", "quest", "DESC")] string desc)
    {
        var quest = QuestBuilder
            .Start("", desc)
            .Build();
        
        Assert.That(quest.Description, Is.EqualTo(desc));
    }

        [Test]
        public void WhenCreateQuest_AndSetAndCompletionType_ThenQuestHasThisType()
    {
        ICompletionType type = new And();
        
        var quest = QuestBuilder
            .Start("", "", null, type)
            .Build();
        
        Assert.That(quest.CompletionType, Is.EqualTo(type));
    }
    
        [Test]
        public void WhenCreateQuest_AndSetOrCompletionType_ThenQuestHasThisType()
    {
        ICompletionType type = new Or();
        
        var quest = QuestBuilder
            .Start("", "", null, type)
            .Build();
        
        Assert.That(quest.CompletionType, Is.EqualTo(type));
    }
    
        [Test]
        public void WhenCreateQuest_AndSetNeededCountCompletionType_ThenQuestHasThisType()
    {
        ICompletionType type = new NeededCount(1);
        
        var quest = QuestBuilder
            .Start("", "", null, type)
            .Build();
        
        Assert.That(quest.CompletionType, Is.EqualTo(type));
    }
    
        [Test]
        public void WhenCreateQuest_AndSetOrderlyProcessor_ThenQuestHasThisType()
    {
        IProcessorType processor = new Orderly();
        
        var quest = QuestBuilder
            .Start("", "", processor, null)
            .Build();
        
        Assert.That(quest.Processor, Is.EqualTo(processor));
    }
    
        [Test]
        public void WhenCreateQuest_AndSetDisorderlyProcessor_ThenQuestHasThisType()
    {
        IProcessorType processor = new Disorderly();
        
        var quest = QuestBuilder
            .Start("", "", processor, null)
            .Build();
        
        Assert.That(quest.Processor, Is.EqualTo(processor));
    }
    }
}