using Karpik.Quests.Interfaces;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class Start
{
    [Test]
    public void WhenStartTask_ThenTaskIsStarted()
    {
        var task = new Task();
        
        task.Start();
        
        Assert.That(task.Status, Is.EqualTo(ITask.TaskStatus.Started));
    }
    
    [Test]
    public void WhenStartTask_ThenTaskCanBeCompleted()
    {
        var task = new Task();
        
        task.Start();
        
        Assert.That(task.CanBeCompleted);
    }
    
    [Test]
    public void WhenStartTask_AndSubscribeStarted_ThenTaskNotifyStarted()
    {
        var task = new Task();
        bool notified = false;

        task.Started += OnTaskStarted;
        task.Start();

        if (notified)
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }

        task.Started -= OnTaskStarted;
        return;
        void OnTaskStarted(ITask task) => notified = true;
    }
}