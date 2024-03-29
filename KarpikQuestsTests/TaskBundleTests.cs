using Karpik.Quests.Interfaces;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Extensions;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

internal class TaskBundleTests
{
    [Test]
    public void WhenTaskBundleIsCreated_AndAddedTasks_ThenBundleHasThem([Values(1, 10, 100)]int count)
    {
        var bundle = new TaskBundle();
        
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"task{i}", $"desc{i}");
            bundle.Add(task);
        }

        Assert.That(bundle, Has.Count.EqualTo(count));
    }

    [Test]
    public void WhenTaskBundleHasANDCompletion_AndNotAllQuestsAreCompleted_ThenBundleIsNotCompleted([Values(10, 100)] int count)
    {
        var bundle = new TaskBundle(CompletionTypesPool.Instance.Pull<And>(), ProcessorTypesPool.Instance.Pull<Orderly>());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"task{i}", $"desc{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        int j = 0;
        foreach (var task in bundle)
        {
            task.TryComplete();
            j++;
            if (j > count / 2)
            {
                break;
            }
        }

        Assert.That(!bundle.IsCompleted());
    }

    [Test]
    public void WhenTaskBundleHasANDCompletion_AndAllQuestsAreCompleted_ThenBundleIsCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(CompletionTypesPool.Instance.Pull<And>(), ProcessorTypesPool.Instance.Pull<Orderly>());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.TryComplete();
        }
        
        Assert.That(bundle.IsCompleted());
    }

    [Test]
    public void WhenTaskBundleHasORCompletion_AndNotAllQuestsAreCompleted_ThenBundleIsCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(CompletionTypesPool.Instance.Pull<Or>(), ProcessorTypesPool.Instance.Pull<Orderly>());
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
            task.TryComplete();
            j++;
            if (j > count / 2)
            {
                break;
            }
        }

        Assert.That(bundle.IsCompleted());
    }

    [Test]
    public void WhenTaskBundleHasORCompletion_AndAllQuestsAreCompleted_ThenBundleIsCompleted([Values(10, 100)] int count)
    {
        ITaskBundle bundle = new TaskBundle(CompletionTypesPool.Instance.Pull<Or>(), ProcessorTypesPool.Instance.Pull<Orderly>());
        for (int i = 0; i < count; i++)
        {
            ITask task = new Task();
            task.Init($"key{i}", $"task{i}");
            bundle.Add(task);
        }
        bundle.Setup();

        foreach (var task in bundle)
        {
            task.TryComplete();
        }

        Assert.That(bundle.IsCompleted());
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
            task.TryComplete();
        }

        Assert.That(bundle.IsCompleted());
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
            task.TryComplete();
            j++;
        }

        Assert.That(!bundle.IsCompleted());
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
            task.TryComplete();
        }

        Assert.That(bundle.IsCompleted());
    }
}
