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
        Assert.Multiple(() =>
        {
            Assert.That(task.Id, Is.EqualTo(clone.Id));
            Assert.That(task.Name, Is.EqualTo(clone.Name));
            Assert.That(task.Description, Is.EqualTo(clone.Description));
            Assert.That(task.Inited, Is.EqualTo(clone.Inited));
            Assert.That(task.Status, Is.EqualTo(clone.Status));
            Assert.That(task.CanBeCompleted, Is.EqualTo(clone.CanBeCompleted));
        });
    }
}