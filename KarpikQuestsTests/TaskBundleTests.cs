using KarpikQuests.Interfaces;
using KarpikQuests.CompletionTypes;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskProcessorTypes;
using NUnit.Framework.Internal;
using Task = KarpikQuests.QuestSample.Task;

namespace KarpikQuestsTests;

internal class TaskBundleTests
{
    [Test]
    public void WhenTaskBundleIsCreated_AndAddedTasks_ThenBundleHasThem([Values(1, 10, 100)]int count)
    {
        ITaskBundle bundle = new TaskBundle();
        
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }

        Assert.That(bundle, Has.Count.EqualTo(count));
    }

    [Test]
    public void WhenTaskBundleHasANDCompletion_AndNotAllQuestsAreCompleted_ThenBundleIsNotCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(new AND(), new Orderly());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        int j = 0;
        foreach (var task in bundle)
        {
            task.TryToComplete();
            j++;
            if (j > count / 2)
            {
                break;
            }
        }

        Assert.That(!bundle.IsCompleted);
    }

    [Test]
    public void WhenTaskBundleHasANDCompletion_AndAllQuestsAreCompleted_ThenBundleIsCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(new AND(), new Orderly());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.TryToComplete();
        }
        
        Assert.That(bundle.IsCompleted);
    }

    [Test]
    public void WhenTaskBundleHasORCompletion_AndNotAllQuestsAreCompleted_ThenBundleIsCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(new OR(), new Orderly());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        int j = 0;
        foreach (var task in bundle)
        {
            task.TryToComplete();
            j++;
            if (j > count / 2)
            {
                break;
            }
        }

        Assert.That(bundle.IsCompleted);
    }

    [Test]
    public void WhenTaskBundleHasORCompletion_AndAllQuestsAreCompleted_ThenBundleIsCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(new OR(), new Orderly());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.TryToComplete();
        }

        Assert.That(bundle.IsCompleted);
    }

    [Test]
    public void WhenTaskBundleHasNeededCountCompletion_AndAllQuestsAreCompleted_ThenBundleIsCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(new NeededCount(1), new Orderly());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.TryToComplete();
        }

        Assert.That(bundle.IsCompleted);
    }

    [Test]
    public void WhenTaskBundleHasNeededCountCompletion_AndCompletedQuestsAreNotEnough_ThenBundleIsNotCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(new NeededCount(count), new Orderly());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        int j = 0;
        foreach (var task in bundle)
        {
            if (j == count / 2)
            {
                break;
            }
            task.TryToComplete();
            j++;
        }

        Assert.That(!bundle.IsCompleted);
    }

    [Test]
    public void WhenTaskBundleHasNeededCountCompletion_AndCompletedQuestsAreEnough_ThenBundleIsCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(new NeededCount(count / 2), new Orderly());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.TryToComplete();
        }

        Assert.That(bundle.IsCompleted);
    }
}
