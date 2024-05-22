using Karpik.Quests.Extensions;
using Karpik.Quests.QuestSample;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests.Quest;

public class ResetQuest
{
    [Test]
    public void WhenCompleteSomeTasks_AndReset_ThenTasksAreUncompleted()
    {
        //Action
        var quest = new QuestSample.Quest();
        var task1 = new Task();
        var task2 = new Task();
        var task3 = new Task();
        var bundle = new TaskBundle();
        bundle.Add(task1);
        bundle.Add(task2);
        bundle.Add(task3);
        quest.Init();
        quest.Add(bundle);
        quest.Start();
        task1.TryComplete();
        task2.TryComplete();

        //Condition
        quest.Reset();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(task1.IsUnStarted());
            Assert.That(task2.IsUnStarted());
            Assert.That(task3.IsUnStarted());
        });
    }
    
    [Test]
    public void WhenCompleteAllTasks_AndReset_ThenTasksAreUncompletedAndQuestStarted()
    {
        //Action
        var quest = new QuestSample.Quest();
        var task1 = new Task();
        var task2 = new Task();
        var task3 = new Task();
        var bundle = new TaskBundle();
        bundle.Add(task1);
        bundle.Add(task2);
        bundle.Add(task3);
        quest.Init();
        quest.Add(bundle);
        quest.Start();
        task1.TryComplete();
        task2.TryComplete();
        task3.TryComplete();

        //Condition
        quest.Reset();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(quest.IsUnStarted());
            Assert.That(task1.IsUnStarted());
            Assert.That(task2.IsUnStarted());
            Assert.That(task3.IsUnStarted());
        });
    }
}