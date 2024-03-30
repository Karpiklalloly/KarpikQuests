using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Extensions;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class CompletionType
{
    [Test]
    public void WhenSetAndType_AndAllTasksAreCompleted_ThenBundleIsCompleted(
        [Values(1, 10, 100)] int count)
    {
        TaskBundle bundle = new TaskBundle(new And(), new Disorderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.ForceComplete();
        }
        
        Assert.That(bundle.IsCompleted());
    }
    
    [Test]
    public void WhenSetOrType_AndSomeTasksAreCompleted_ThenBundleIsCompleted(
        [Values(1, 10, 100)] int count)
    {
        int completeCount = count / 2 + 1;
        TaskBundle bundle = new TaskBundle(new Or(), new Disorderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.ForceComplete();
            completeCount--;
            if (completeCount == 0) break;
        }
        
        Assert.That(bundle.IsCompleted());
    }
    
    [Test]
    public void WhenSetAndType_AndSomeTasksAreFailed_ThenBundleIsFailed(
        [Values(1, 10, 100)] int count)
    {
        TaskBundle bundle = new TaskBundle(new And(), new Disorderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();
        
        bundle.First().ForceFail();
        
        Assert.That(bundle.IsFailed());
    }
    
    [Test]
    public void WhenSetOrType_AndNotAllTasksAreFailed_ThenBundleIsCompleted(
        [Values(2, 10, 100)] int count)
    {
        TaskBundle bundle = new TaskBundle(new Or(), new Disorderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();

        bundle.First().TryFail();
        foreach (var task in bundle)
        {
            task.TryComplete();
        }
        
        Assert.That(bundle.IsCompleted());
    }
    
    [Test]
    public void WhenSetNeededCountType_AndTasksAreCompletedMoreThanNeeded_ThenBundleIsCompleted(
        [Values(2, 10, 100)] int count,
        [Values(1, 2)] int needed)
    {
        int neededCount = needed;
        TaskBundle bundle = new TaskBundle(new NeededCount(needed), new Disorderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();
        
        foreach (var task in bundle)
        {
            task.TryComplete();
            neededCount--;
            if (neededCount == 0) break;
        }
        
        Assert.That(bundle.IsCompleted());
    }
    
    [Test]
    public void WhenSetNeededCountType_AndTasksAreCompletedLessThanNeededButAtleastOne_ThenBundleIsStarted(
        [Values(2, 10, 100)] int count)
    {
        int neededCount = 1;
        TaskBundle bundle = new TaskBundle(new NeededCount(neededCount + 1), new Disorderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();
        
        foreach (var task in bundle)
        {
            if (neededCount == 0) break;
            task.TryComplete();
            neededCount--;
        }
        
        Assert.That(bundle.IsStarted());
    }
    
    [Test]
    public void WhenSetNeededCountType_AndTasksAreFailedTooMuch_ThenBundleIsFailed(
        [Values(2, 10, 100)] int count)
    {
        int neededCount = count - 1;
        int cycles = count / 2 + 1;
        TaskBundle bundle = new TaskBundle(new NeededCount(neededCount), new Disorderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();
        
        foreach (var task in bundle)
        {
            if (cycles == 0) break;
            task.TryFail();
            cycles--;
        }
        
        Assert.That(bundle.IsFailed());
    }
}