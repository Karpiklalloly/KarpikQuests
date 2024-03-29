using Karpik.Quests.Interfaces;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class Reset
{
    [Test]
    public void WhenResetTask_ThenTaskCanNotBeCompleted()
    {
        var task = new Task();
        task.Start();
        
        task.Reset();
        
        Assert.That(!task.CanBeCompleted);
    }
    
    [Test]
    public void WhenResetTask_ThenTaskIsUnStarted()
    {
        var task = new Task();
        task.Start();
        
        task.Reset();
        
        Assert.That(task.Status, Is.EqualTo(ITask.TaskStatus.UnStarted));
    }
    
    [Test]
    public void WhenResetAndStart_AndSubscribeStarted_ThenTaskDoNotNotifyStarted()
    {
        var task = new Task();
        bool notified = false;
        
        task.Started += OnTaskStarted;
        task.Reset();
        task.Start();

        if (notified)
        {
            Assert.Fail();
        }
        else
        {
            Assert.Pass();
        }

        return;

        void OnTaskStarted(ITask task) => notified = true;
    }
    
    [Test]
    public void WhenResetAndTryCompleteTask_AndSubscribeCompleted_ThenTaskNotNotifyCompleted()
    {
        bool notified = false;
        var task = new Task();
        task.Start();

        task.Started += OnTaskStarted;
        task.Reset();
        task.Start();
        task.TryComplete();
        
        if (notified)
        {
            Assert.Fail();
        }
        else
        {
            Assert.Pass();
        }

        return;
        void OnTaskStarted(ITask task) => notified = true;
    }
    
    [Test]
    public void WhenResetAndTryFailTask_AndSubscribeFailed_ThenTaskNotNotifyFailed()
    {
        bool notified = false;
        var task = new Task();
        task.Start();
        task.Failed += OnTaskFailed;
        
        task.Reset();
        task.Start();
        task.TryFail();
        
        if (notified)
        {
            Assert.Fail();
        }
        else
        {
            Assert.Pass();
        }

        return;
        void OnTaskFailed(ITask task) => notified = true;
    }
}