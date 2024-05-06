using Karpik.Quests.QuestSample;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests.Quest;

public class AddTaskQuest
{
    [Test]
    public void WhenCreateQuest_AndAddTask_ThenQuestHasThisTask()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var task = new Task();

        //Condition
        quest.Add(task);

        //Result
        Assert.That(quest.TaskBundles.Has(task));
    }

    [Test]
    public void WhenCreateQuest_AndAddBundle_ThenQuestHasThisBundle()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var bundle = new TaskBundle();

        //Condition
        quest.Add(bundle);

        //Result
        Assert.That(quest.TaskBundles.Has(bundle));
    }
    
    [Test]
    public void WhenCreateQuest_AndAddBundleWithTask_ThenQuestHasThisTask()
    {
        //Action
        var quest = new QuestSample.Quest();
        quest.Init();
        var bundle = new TaskBundle();
        var task = new Task();
        bundle.Add(task);

        //Condition
        quest.Add(bundle);

        //Result
        Assert.That(quest.TaskBundles.Has(task));
    }
}