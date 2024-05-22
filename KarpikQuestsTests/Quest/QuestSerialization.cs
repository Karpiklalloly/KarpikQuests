using Karpik.Quests.Extensions;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.Saving;
using Task = Karpik.Quests.QuestSample.Task;

namespace Karpik.Quests.Tests.Quest;

public class QuestSerialization
{
    [Test]
    public void WhenSerializeQuest_AndDeserialize_ThenTheyAreEqual()
    {
        //Action
        var bundle1 = new TaskBundle(); bundle1.Add(new Task()); bundle1.Add(new Task());
        var bundle2 = new TaskBundle(); bundle2.Add(new Task()); bundle2.Add(new Task());
        var quest = QuestBuilder.Start<QuestSample.Quest>("Name", "dfgfmbkltdnlkmlk ☺♣♠")
            .AddBundle(bundle1)
            .AddBundle(bundle2)
            .Build();

        var i = 1;
        foreach (var bundle in quest.TaskBundles)
        {
            foreach (var task in bundle)
            {
                task.Init($"{i + 1}", "descripttttt");
                i += i;
            }
        }
        quest.TaskBundles[1].Setup();
        
        var serializer = new JsonResolver<QuestSample.Quest>();
        var json = serializer.Serialize(quest as QuestSample.Quest);
        
        //Condition
        var clone = serializer.Deserialize(json);
        
        //Result
        AssetFullEqual(quest, clone);
    }

    public static void AssetFullEqual(IQuest quest, IQuest clone)
    {
        Assert.That(quest.Id, Is.EqualTo(clone.Id));
        Assert.That(quest.Name, Is.EqualTo(clone.Name));
        Assert.That(quest.Description, Is.EqualTo(clone.Description));
        Assert.That(quest.Inited, Is.EqualTo(clone.Inited));
        Assert.That(quest.TaskBundles, Has.Count.EqualTo(clone.TaskBundles.Count));
        Assert.That(quest.CompletionType.GetType(), Is.EqualTo(clone.CompletionType.GetType()));
        Assert.That(quest.Processor.GetType(), Is.EqualTo(clone.Processor.GetType()));
        Assert.That(quest.Status.GetType(), Is.EqualTo(clone.Status.GetType()));
        for (int j = 0; j < quest.TaskBundles.Count; j++)
        {
            var orig = quest.TaskBundles[j];
            var c = clone.TaskBundles[j];
            BundleSerialization.AssertFullEqual(orig as TaskBundle, c as TaskBundle);
        }
    }

    [Test]
    public void WhenSerialize_AndDeserialize_ThenCompletedWork()
    {
        //Action
        var bundle1 = new TaskBundle(); bundle1.Add(new Task()); bundle1.Add(new Task());
        var bundle2 = new TaskBundle(); bundle2.Add(new Task()); bundle2.Add(new Task());
        var quest = QuestBuilder.Start<QuestSample.Quest>("Name", "dfgfmbkltdnlkmlk ☺♣♠")
            .AddBundle(bundle1)
            .AddBundle(bundle2)
            .Build();
        bool flag = false;
        
        var serializer = new JsonResolver<QuestSample.Quest>();
        var json = serializer.Serialize(quest as QuestSample.Quest);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Completed += quest1 => flag = true;
        foreach (var bundle in clone.TaskBundles)
        {
            foreach (var task in bundle)
            {
                task.ForceComplete();
            }
        }

        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerialize_AndDeserialize_ThenFailedWork()
    {
        //Action
        var bundle1 = new TaskBundle(); bundle1.Add(new Task()); bundle1.Add(new Task());
        var bundle2 = new TaskBundle(); bundle2.Add(new Task()); bundle2.Add(new Task());
        var quest = QuestBuilder.Start<QuestSample.Quest>("Name", "dfgfmbkltdnlkmlk ☺♣♠")
            .AddBundle(bundle1)
            .AddBundle(bundle2)
            .Build();
        bool flag = false;
        
        var serializer = new JsonResolver<QuestSample.Quest>();
        var json = serializer.Serialize(quest as QuestSample.Quest);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Failed += quest1 => flag = true;
        foreach (var bundle in clone.TaskBundles)
        {
            foreach (var task in bundle)
            {
                task.ForceFail();
            }
        }

        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerialize_AndDeserialize_ThenUpdatedWork()
    {
        //Action
        var bundle1 = new TaskBundle(); bundle1.Add(new Task()); bundle1.Add(new Task());
        var bundle2 = new TaskBundle(); bundle2.Add(new Task()); bundle2.Add(new Task());
        var quest = QuestBuilder.Start<QuestSample.Quest>("Name", "dfgfmbkltdnlkmlk ☺♣♠")
            .AddBundle(bundle1)
            .AddBundle(bundle2)
            .Build();
        bool flag = false;
        
        var serializer = new JsonResolver<QuestSample.Quest>();
        var json = serializer.Serialize(quest as QuestSample.Quest);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Updated += (quest1, bundle) => flag = true;
        clone.TaskBundles[0].Tasks.First().ForceComplete();

        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerialize_AndDeserialize_ThenStartedWork()
    {
        //Action
        var bundle1 = new TaskBundle(); bundle1.Add(new Task()); bundle1.Add(new Task());
        var bundle2 = new TaskBundle(); bundle2.Add(new Task()); bundle2.Add(new Task());
        var quest = QuestBuilder.Start<QuestSample.Quest>("Name", "dfgfmbkltdnlkmlk ☺♣♠")
            .AddBundle(bundle1)
            .AddBundle(bundle2)
            .Build();
        bool flag = false;
        
        var serializer = new JsonResolver<QuestSample.Quest>();
        var json = serializer.Serialize(quest as QuestSample.Quest);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.Started += (quest1) => flag = true;
        clone.Start();

        //Result
        Assert.That(flag);
    }
}