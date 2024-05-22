using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Factories;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.Statuses;
using Karpik.Quests.TaskProcessorTypes;

namespace Karpik.Quests.Tests.Quest;

public class InitQuest
{
    [Test]
    public void WhenInitQuest_AndSetProcess_ThenQuestHasThisProcessor()
    {
        //Action
        IProcessorType processor = new Disorderly();
        var quest = new QuestSample.Quest();

        //Condition
        quest.Init("name", "description", new TaskBundleCollection(), new And(), processor);

        //Result
        Assert.That(quest.Processor.GetType(), Is.EqualTo(processor.GetType()));
    }
    
    [Test]
    public void WhenInitQuest_AndSetProcess_ThenQuestHasThisProcessor2()
    {
        //Action
        IProcessorType processor = new Orderly();
        var quest = new QuestSample.Quest();

        //Condition
        quest.Init("name", "description", new TaskBundleCollection(), new And(), processor);

        //Result
        Assert.That(quest.Processor.GetType(), Is.EqualTo(processor.GetType()));
    }
    
    [Test]
    public void WhenInitQuest_AndSetCompletionType_ThenQuestHasThisCompletionType()
    {
        //Action
        ICompletionType completion = new And();
        var quest = new QuestSample.Quest();

        //Condition
        quest.Init("name", "description", new TaskBundleCollection(), completion, new Orderly());

        //Result
        Assert.That(quest.CompletionType.GetType(), Is.EqualTo(completion.GetType()));
    }
    
    [Test]
    public void WhenInitQuest_AndSetCompletionType_ThenQuestHasThisCompletionType2()
    {
        //Action
        ICompletionType completion = new Or();
        var quest = new QuestSample.Quest();

        //Condition
        quest.Init("name", "description", new TaskBundleCollection(), completion, new Orderly());

        //Result
        Assert.That(quest.CompletionType.GetType(), Is.EqualTo(completion.GetType()));
    }
    
    [Test]
    public void WhenInitQuest_AndSetCompletionType_ThenQuestHasThisCompletionType3()
    {
        //Action
        ICompletionType completion = new NeededCount(123);
        var quest = new QuestSample.Quest();

        //Condition
        quest.Init("name", "description", new TaskBundleCollection(), completion, new Orderly());

        //Result
        Assert.That(quest.CompletionType.GetType(), Is.EqualTo(completion.GetType()));
    }

    [Test]
    public void WhenInitDefault_ThenValuesAreDefault()
    {
        //Action
        var quest = new QuestSample.Quest();

        //Condition
        quest.Init();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(quest.Name, Is.EqualTo("Quest"));
            Assert.That(quest.Description, Is.EqualTo("Description"));
            Assert.That(quest.TaskBundles, Is.Empty);
            Assert.That(quest.TaskBundles.GetType(), Is.EqualTo(TaskBundleCollectionFactory.Instance.Create().GetType()));
            Assert.That(quest.Status is UnStarted);
            Assert.That(quest.CompletionType.GetType(), Is.EqualTo(CompletionTypesFactory.Instance.Create().GetType()));
            Assert.That(quest.Processor.GetType(), Is.EqualTo(ProcessorFactory.Instance.Create().GetType()));
        });
    }

    [Test]
    public void WhenCreateQuest_AndInit_ThenQuestIsInited()
    {
        //Action
        var quest = new QuestSample.Quest();

        //Condition
        quest.Init();

        //Result
        Assert.That(quest.Inited);
    }
}