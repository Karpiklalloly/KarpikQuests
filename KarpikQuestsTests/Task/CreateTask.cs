using Karpik.Quests.ID;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class CreateTask
{
    [Test]
    public void WhenCreateTask_AndSetCorrectId_ThenTaskHasThisId(
        [Values("key", "task", "KEY")] string key)
    {
        string correctKey = key;
        
        var task = new Task(new Id(correctKey));
        
        Assert.That(task.Id.Value, Is.EqualTo(correctKey));
    }

    [Test]
    public void WhenCreateTask_AndSetIncorrectId_ThenTaskHasNoThisId(
        [Values("", " ", "\n", "\t")] string key)
    {
        string incorrectKey = key;
        
        var task = new Task(new Id(incorrectKey));
        
        Assert.That(task.Id.Value, Is.Not.EqualTo(incorrectKey));
    }
    
    [Test]
    public void WhenCreateTask_AndSetIncorrectId_ThenTaskHasEmptyId(
        [Values("", " ", "\n", "\t")] string key)
    {
        string incorrectKey = key;
        
        var task = new Task(new Id(incorrectKey));
        
        Assert.That(task.Id, Is.EqualTo(Id.Empty));
    }

    [Test]
    public void WhenCreateTask_AndSetEmptyKey_ThenTaskHasEmptyKey()
    {
        string key = "-1";

        var task = new Task(new Id(key));
        
        Assert.That(task.Id, Is.EqualTo(Id.Empty));
    }
}