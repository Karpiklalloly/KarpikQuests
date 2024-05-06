using Karpik.Quests.QuestSample;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests.Quest;

public class RemoveQuest
{
    [Test]
    public void WhenAddBundle_AndRemoveBundle_ThenQuestDoesNotHaveBundle()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var bundle = new TaskBundle();
        quest.Add(bundle);

        //Condition
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.TaskBundles.Has(bundle));
    }
    
    [Test]
    public void WhenAddBundle_AndRemoveBundle_ThenQuestDoesNotHaveBundle2()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var bundle = new TaskBundle();
        quest.Add(new TaskBundle());
        quest.Add(bundle);

        //Condition
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.TaskBundles.Has(bundle));
    }
    
    [Test]
    public void WhenAddTask_AndRemoveTask_ThenQuestDoesNotHaveTask()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var task = new Task();
        quest.Add(task);

        //Condition
        quest.Remove(task);

        //Result
        Assert.That(!quest.Has(task));
    }
    
    [Test]
    public void WhenAddTask_AndRemoveBundle_ThenQuestDoesNotHaveTask()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var task = new Task();
        quest.Add(task);

        //Condition
        quest.Remove(quest.TaskBundles[0]);

        //Result
        Assert.That(!quest.Has(task));
    }
    
    [Test]
    public void WhenRemoveBundleNotAdded_ThenQuestDoesNotHaveBundle()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var bundle = new TaskBundle();

        //Condition
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.Has(bundle));
    }
    
    [Test]
    public void WhenRemoveTaskNotAdded_ThenQuestDoesNotHaveTask()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var task = new Task();

        //Condition
        quest.Remove(task);

        //Result
        Assert.That(!quest.Has(task));
    }
    
    [Test]
    public void WhenRemoveBundleFromEmptyQuest_ThenQuestDoesNotHaveBundle()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var bundle = new TaskBundle();

        //Condition
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.TaskBundles.Has(bundle));
    }
    
    [Test]
    public void WhenRemoveBundleMultipleTimes_ThenQuestDoesNotHaveBundle()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var bundle = new TaskBundle();
        quest.Add(bundle);

        //Condition
        quest.Remove(bundle);
        quest.Remove(bundle);

        //Result
        Assert.That(!quest.TaskBundles.Has(bundle));
    }
    
    [Test]
    public void WhenRemoveTaskMultipleTimes_ThenQuestDoesNotHaveTask()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var task = new Task();
        quest.Add(task);

        //Condition
        quest.Remove(task);
        quest.Remove(task);

        //Result
        Assert.That(!quest.Has(task));
    }
}