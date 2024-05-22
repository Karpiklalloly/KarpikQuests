using Karpik.Quests.Interfaces;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class TryComplete
{
    [Test]
    public void WhenStartTask_AndTryComplete_ThenTaskIsCompleted()
    {
        var task = new Task();
        task.Start();
        
        task.TryComplete();
        
        Assert.That(task.Status, Is.EqualTo(ITask.TaskStatus.Completed));
    }
    
    [Test]
    public void WhenStartAndTryCompleteTask_AndSubscribeCompleted_ThenTaskNotifyCompleted()
    {
        bool notified = false;
        var task = new Task();
        task.Start();

        task.Completed += OnTaskCompleted;
        task.TryComplete();
        
        if (notified)
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }

        task.Completed -= OnTaskCompleted;
        return;
        void OnTaskCompleted(ITask task) => notified = true;
    }
}