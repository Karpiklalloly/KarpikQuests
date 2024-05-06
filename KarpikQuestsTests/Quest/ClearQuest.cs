using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests.Quest;

public class ClearQuest
{
    [Test]
    public void WhenAddTasks_AndClearQuest_ThenQuestDoesNotHaveThem()
    {
        //Action
        var quest = new QuestSample.Quest();
        var task1 = new Task();
        var task2 = new Task();
        quest.Init();
        quest.Add(task1);
        quest.Add(task2);

        //Condition
        quest.Clear();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(!quest.Has(task1));
            Assert.That(!quest.Has(task2));
            Assert.That(quest.TaskBundles.Count == 0);
        });
    }
    
    [Test]
    public void WhenAddTasks_AndClearQuest_ThenAfterCompleteThemAllQuestIsNotCompleted()
    {
        //Action
        var quest = new QuestSample.Quest();
        var task1 = new Task();
        var task2 = new Task();
        quest.Init();
        quest.Add(task1);
        quest.Add(task2);
        bool completed = false;
        bool updated = false;

        //Condition
        quest.Completed += quest1 => completed = true;
        quest.Updated += (quest1, bundle) => updated = true;
        
        quest.Clear();
        task1.ForceComplete();
        task2.ForceComplete();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(!updated);
            Assert.That(!completed);
        });
    }
}