using Karpik.Quests;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Processors;

namespace KarpikQuestsTests.QuestTests
{
    public class CreateQuest
    {
        [Test]
        public void WhenCreateQuest_AndSetCorrectId_ThenQuestHasThisId()
    {
        //Action
        Id id = Id.NewId();

        //Condition
        var quest = new Quest(id);

        //Result
        Assert.That(quest.Id, Is.EqualTo(id));
    }

        [Test]
        public void WhenCreateQuest_AndSetProcess_ThenQuestHasThisProcessor()
    {
        //Action
        IProcessorType processor = new Disorderly();
        

        //Condition
        var quest = new Quest(Id.NewId(), "name", "description", null, processor);

        //Result
        Assert.That(quest.Processor.GetType(), Is.EqualTo(processor.GetType()));
    }
    
        [Test]
        public void WhenCreateQuest_AndSetProcess_ThenQuestHasThisProcessor2()
    {
        //Action
        IProcessorType processor = new Orderly();

        //Condition
        var quest = new Quest(Id.NewId(), "name", "description", null, processor);

        //Result
        Assert.That(quest.Processor.GetType(), Is.EqualTo(processor.GetType()));
    }
    
        [Test]
        public void WhenCreateQuest_AndSetCompletionType_ThenQuestHasThisCompletionType()
    {
        //Action
        ICompletionType completion = new And();

        //Condition
        var quest = new Quest(Id.NewId(), "name", "description", completion, null);

        //Result
        Assert.That(quest.CompletionType.GetType(), Is.EqualTo(completion.GetType()));
    }
    
        [Test]
        public void WhenCreateQuest_AndSetCompletionType_ThenQuestHasThisCompletionType2()
    {
        //Action
        ICompletionType completion = new Or();

        //Condition
        var quest = new Quest(Id.NewId(), "name", "description", completion, null);

        //Result
        Assert.That(quest.CompletionType.GetType(), Is.EqualTo(completion.GetType()));
    }
    
        [Test]
        public void WhenCreateQuest_AndSetCompletionType_ThenQuestHasThisCompletionType3()
    {
        //Action
        ICompletionType completion = new NeededCount(123);

        //Condition
        var quest = new Quest(Id.NewId(), "name", "description", completion, null);

        //Result
        Assert.That(quest.CompletionType.GetType(), Is.EqualTo(completion.GetType()));
    }

        [Test]
        public void WhenCreateDefault_ThenValuesAreDefault()
    {
        //Action
        var quest = new Quest();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(quest.Name, Is.EqualTo("Quest"));
            Assert.That(quest.Description, Is.EqualTo("Description"));
            Assert.That(quest.Status == Status.Locked);
            Assert.That(quest.CompletionType.GetType(), Is.EqualTo(new And().GetType()));
            Assert.That(quest.Processor.GetType(), Is.EqualTo(new Disorderly().GetType()));
        });
    }
    }
}