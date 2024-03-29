using Karpik.Quests.Interfaces;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class Setup
{
    [Test]
    public void WhenSetupTask_ThenTaskIsUnStarted()
    {
        var task = new Task();
        
        task.Setup();
        
        Assert.That(task.Status, Is.EqualTo(ITask.TaskStatus.UnStarted));
    }
    
    [Test]
    public void WhenSetupTask_ThenTaskCanNotBeCompleted()
    {
        var task = new Task();
        
        task.Setup();
        
        Assert.That(!task.CanBeCompleted);
    }
}