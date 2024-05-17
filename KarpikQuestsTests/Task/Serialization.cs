using Karpik.Quests.Interfaces;
using Karpik.Quests.Saving;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests;

public class Serialization
{
    [Test]
    public void WhenSerializeTask_AndDeserializeToAnotherTask_ThenTheyAreFullEqual()
    {
        //Action
        var task = new Task();
        task.Init("My super name =+-_)(*&^%$#@!☺", 
            "My bad description☻");
        var serializer = new JsonResolver<ITask>();
        var json = serializer.Serialize(task);
        
        //Condition
        var clone = serializer.Deserialize(json);
        
        //Result
        AssertFullEqual(task, clone);
    }

    public static void AssertFullEqual(ITask task, ITask clone)
    {
        Assert.That(task.Id, Is.EqualTo(clone.Id));
        Assert.That(task.Name, Is.EqualTo(clone.Name));
        Assert.That(task.Description, Is.EqualTo(clone.Description));
        Assert.That(task.Inited, Is.EqualTo(clone.Inited));
        Assert.That(task.Status, Is.EqualTo(clone.Status));
        Assert.That(task.CanBeCompleted, Is.EqualTo(clone.CanBeCompleted));
    }

    [Test]
    public void WhenSerialize_AndDeserialize_ThenCompletedWork()
    {
        //Action
        var task = new Task();
        task.Init("My super name =+-_)(*&^%$#@!☺", 
            "My bad description☻");
        bool flag = false;
        var serializer = new JsonResolver<ITask>();
        var json = serializer.Serialize(task);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Completed += (task) => { flag = true; };
        clone.ForceComplete();
        
        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerialize_AndDeserialize_ThenFailedWork()
    {
        //Action
        var task = new Task();
        task.Init("My super name =+-_)(*&^%$#@!☺", 
            "My bad description☻");
        bool flag = false;
        var serializer = new JsonResolver<ITask>();
        var json = serializer.Serialize(task);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Failed += (task) => { flag = true; };
        clone.ForceFail();
        
        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerialize_AndDeserialize_ThenStartedWork()
    {
        //Action
        var task = new Task();
        task.Init("My super name =+-_)(*&^%$#@!☺", 
            "My bad description☻");
        bool flag = false;
        var serializer = new JsonResolver<ITask>();
        var json = serializer.Serialize(task);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Started += (task) => { flag = true; };
        clone.Start();
        
        //Result
        Assert.That(flag);
    }
}