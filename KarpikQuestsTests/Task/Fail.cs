using Karpik.Quests.Interfaces;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class Fail
{
    [Test]
    public void WhenStartTask_AndTryFail_ThenTaskIsFailed()
    {
        var task = new Task();
        task.Start();
        
        task.TryFail();
        
        Assert.That(task.Status, Is.EqualTo(ITask.TaskStatus.Failed));
    }
    
    [Test]
    public void WhenTryFailTask_AndSubscribeFailed_ThenTaskNotifyFailed()
    {
        bool notified = false;
        var task = new Task();
        task.Start();
        task.Failed += OnTaskFailed;
        
        task.TryFail();
        
        if (notified)
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }

        task.Failed -= OnTaskFailed;
        return;
        void OnTaskFailed(ITask task) => notified = true;
    }
}