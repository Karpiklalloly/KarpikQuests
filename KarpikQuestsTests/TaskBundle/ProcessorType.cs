using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Extensions;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class ProcessorType
{
    [Test]
    public void WhenSetOrderlyType_AndQuestsCompletesByOrder_ThenBundleIsCompleted(
        [Values(2, 10, 100)] int count)
    {
        TaskBundle bundle = new TaskBundle(new And(), new Orderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.TryComplete();
        }
        
        Assert.That(bundle.IsCompleted());
    }
    
    [Test]
    public void WhenSetOrderlyType_AndQuestsCompletesNonByOrder_ThenBundleIsStarted(
        [Values(2, 10, 100)] int count)
    {
        TaskBundle bundle = new TaskBundle(new And(), new Orderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();
        
        foreach (var task in bundle)
        {
            if (task.Equals(bundle.Tasks.First())) continue;
            task.TryComplete();
        }
        bundle.Tasks.First().TryComplete();
        
        Assert.That(bundle.IsStarted());
    }
    
    [Test]
    public void WhenSetOrderlyType_AndMiddleQuestForCompleted_ThenNextQuestIsNotStarted(
        [Values(3, 10, 100)] int count)
    {
        TaskBundle bundle = new TaskBundle(new And(), new Orderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();

        bundle.Tasks.ElementAt(bundle.Count / 2 + 1).ForceComplete();
        
        Assert.That(!bundle.Tasks.ElementAt(bundle.Count / 2).IsStarted());
    }
    
    [Test]
    public void WhenSetDisorderlyType_AndQuestsCompletesByOrder_ThenBundleIsCompleted(
        [Values(2, 10, 100)] int count)
    {
        TaskBundle bundle = new TaskBundle(new And(), new Disorderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.TryComplete();
        }
        
        Assert.That(bundle.IsCompleted());
    }
    
    [Test]
    public void WhenSetDisorderlyType_AndQuestsCompletesNonByOrder_ThenBundleIsCompleted(
        [Values(2, 10, 100)] int count)
    {
        TaskBundle bundle = new TaskBundle(new And(), new Disorderly());
        for (int i = 0; i < count; i++)
        {
            bundle.Add(new Task());
        }
        bundle.Setup();
        
        foreach (var task in bundle)
        {
            if (task.Equals(bundle.Tasks.First())) continue;
            task.TryComplete();
        }
        bundle.Tasks.First().TryComplete();
        
        Assert.That(bundle.IsCompleted());
    }
}