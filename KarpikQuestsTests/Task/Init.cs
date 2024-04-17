using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class Init
{
    [Test]
    public void WhenInitTask_AndSetCorrectKeyNameDescription_ThenTheyAreEqualToTask(
        [Values("key", "task", "KEY")]   string key,
        [Values("name", "task", "NAME", "___", "!7546&((**%&^%&6")] string name,
        [Values("desc", "task", "DESC")] string desc)
    {
        var task = new Task(new Id(key));

        task.Init(name, desc);
        
        Assert.Multiple(() =>
        {
            Assert.That(task.Id.Value, Is.EqualTo(key));
            Assert.That(task.Name, Is.EqualTo(name));
            Assert.That(task.Description, Is.EqualTo(desc));
        });
    }
    
    [Test]
    public void WhenInitTask_AndSetCorrectName_ThenTaskHasThisName(
        [Values("name", "task", "NAME", "___", "!7546&((**%&^%&6")] string name)
    {
        var task = new Task();
        
        task.Init(name, "");
        
        Assert.That(name, Is.EqualTo(task.Name));
    }
    
    [Test]
    public void WhenInitTask_AndSetInCorrectName_ThenTaskHasNoThisName(
        [Values("", "\t", "\n", null)] string name)
    {
        var task = new Task();
        
        task.Init(name, "");
        
        Assert.Multiple(() =>
        {
            Assert.That(name, Is.Not.EqualTo(task.Name));
            Assert.That(task.Name, Is.EqualTo("Task"));
        });
    }

    [Test]
    public void WhenInitTask_AndSetCorrectDescription_ThenTaskHasThisDescription(
        [Values("desc", "___", "", "\t", "\n")] string desc)
    {
        var task = new Task();
        
        task.Init("Key", desc);
        
        Assert.That(desc, Is.EqualTo(task.Description));
    }
    
    [Test]
    public void WhenInitTask_AndSetInCorrectDescription_ThenTaskHasNoThisDescription(
        [Values(null)] string desc)
    {
        var task = new Task();
        
        task.Init("Key", desc);
        
        Assert.Multiple(() =>
        {
            Assert.That(desc, Is.Not.EqualTo(task.Description));
            Assert.That(task.Description, Is.EqualTo("Description"));
        });
    }
    
    [Test]
    public void WhenInitTask_ThenTaskIsUnStarted()
    {
        var task = new Task();
        
        task.Init();
        
        Assert.That(task.Status, Is.EqualTo(ITask.TaskStatus.UnStarted));
    }
}