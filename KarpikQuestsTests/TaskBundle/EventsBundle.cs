using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Extensions;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class EventsBundle
{
    [Test]
    public void WhenCompleteTask_ThenBundleUpdated()
    {
        //Action
        var bundle = new TaskBundle();
        bundle.Add(new Task());
        bundle.Add(new Task());
        bool updated = false;
        bundle.Updated += bundle => updated = true;

        //Condition
        bundle[0].ForceComplete();

        //Result
        Assert.That(updated);
    }

    [Test]
    public void WhenBundleHasAnd_AndAllTasksAreCompleted_ThenBundleCompleted()
    {
        //Action
        var bundle = new TaskBundle(new And(), new Disorderly());
        bundle.Add(new Task());
        bundle.Add(new Task());
        bool completed = false;
        bundle.Completed += bundle => completed = true;

        //Condition
        foreach (var task in bundle)
        {
            task.ForceComplete();
        }

        //Result
        Assert.That(completed);
    }
    
    [Test]
    public void WhenBundleHasAnd_AndOneTaskIsFailed_ThenBundleFailed()
    {
        //Action
        var bundle = new TaskBundle(new And(), new Disorderly());
        bundle.Add(new Task());
        bundle.Add(new Task());
        bool failed = false;
        bundle.Failed += bundle => failed = true;

        //Condition
        bundle[0].ForceComplete();
        bundle[1].ForceFail();

        //Result
        Assert.That(failed);
    }
    
    [Test]
    public void WhenBundleHasOr_AndOneOfTheTasksIsCompleted_ThenBundleCompleted()
    {
        //Action
        var bundle = new TaskBundle(new Or(), new Disorderly());
        bundle.Add(new Task());
        bundle.Add(new Task());
        bool completed = false;
        bundle.Completed += bundle => completed = true;

        //Condition
        bundle[0].ForceComplete();

        //Result
        Assert.That(completed);
    }
    
    [Test]
    public void WhenBundleHasOr_AndAllTasksAreFailed_ThenBundleFailed()
    {
        //Action
        var bundle = new TaskBundle(new Or(), new Disorderly());
        bundle.Add(new Task());
        bundle.Add(new Task());
        bool failed = false;
        bundle.Failed += bundle => failed = true;

        //Condition
        foreach (var task in bundle)
        {
            task.ForceFail();
        }

        //Result
        Assert.That(failed);
    }
    
    [Test]
    public void WhenBundleHasOrderly_AndAllTasksAreCompletedByOrder_ThenBundleCompleted()
    {
        //Action
        var bundle = new TaskBundle(new And(), new Orderly());
        bundle.Add(new Task());
        bundle.Add(new Task());
        bundle.Setup();

        //Condition
        foreach (var task in bundle)
        {
            task.TryComplete();
        }

        //Result
        Assert.That(bundle.IsCompleted());
    }
    
    [Test]
    public void WhenBundleHasOrderly_AndTasksAreCompletedByDisorder_ThenBundleStarted()
    {
        //Action
        var bundle = new TaskBundle(new And(), new Orderly());
        bundle.Add(new Task());
        bundle.Add(new Task());
        bundle.Setup();

        //Condition
        bundle[1].TryComplete();
        bundle[0].TryComplete();

        //Result
        Assert.That(bundle.IsStarted());
    }
}