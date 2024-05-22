using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.Saving;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class BundleSerialization
{
    [Test]
    public void WhenSerializeBundle_AndDeserialize_ThenParamsAreEqual()
    {
        //Action
        var bundle = new TaskBundle();
        bundle.Add(new Task());
        var task = new Task();
        task.Init("name", "desc");
        bundle.Add(task);
        var serializer = new JsonResolver<ITaskBundle>();
        var json = serializer.Serialize(bundle);
        
        //Condition
        var clone = serializer.Deserialize(json);
        
        //Result
        AssertFullEqual(bundle, clone as TaskBundle);
    }

    public static void AssertFullEqual(TaskBundle bundle, TaskBundle clone)
    {
        Assert.That(bundle, Has.Count.EqualTo(clone.Count));
        Assert.That(bundle.CompletionType.GetType(), Is.EqualTo(clone.CompletionType.GetType()));
        Assert.That(bundle.Processor.GetType(), Is.EqualTo(clone.Processor.GetType()));
        Assert.That(bundle.Status.GetType(), Is.EqualTo(clone.Status.GetType()));
        for (int i = 0; i < bundle.Count; i++)
        {
            Serialization.AssertFullEqual(bundle[i], clone[i]);
        }
    }

    [Test]
    public void WhenSerialize_AndDeserialize_ThenCompletedWork()
    {
        //Action
        var bundle = new TaskBundle();
        bundle.Add(new Task());
        var task = new Task();
        task.Init("name", "desc");
        bundle.Add(task);
        bool flag = false;
        
        var serializer = new JsonResolver<ITaskBundle>();
        var json = serializer.Serialize(bundle);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Completed += (bundle) => { flag = true; };
        foreach (var task2 in clone.Tasks)
        {
            task2.ForceComplete();
        }
        
        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerialize_AndDeserialize_ThenFailedWork()
    {
        //Action
        var bundle = new TaskBundle();
        bundle.Add(new Task());
        var task = new Task();
        task.Init("name", "desc");
        bundle.Add(task);
        bool flag = false;
        
        var serializer = new JsonResolver<ITaskBundle>();
        var json = serializer.Serialize(bundle);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Failed += (bundle) => { flag = true; };
        foreach (var task2 in clone.Tasks)
        {
            task2.ForceFail();
        }
        
        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerialize_AndDeserialize_ThenUpdatedWork()
    {
        //Action
        var bundle = new TaskBundle();
        bundle.Add(new Task());
        var task = new Task();
        task.Init("name", "desc");
        bundle.Add(task);
        bool flag = false;
        
        var serializer = new JsonResolver<ITaskBundle>();
        var json = serializer.Serialize(bundle);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Updated += (bundle) => { flag = true; };
        clone.Tasks.First().ForceComplete();
        
        //Result
        Assert.That(flag);
    }
}