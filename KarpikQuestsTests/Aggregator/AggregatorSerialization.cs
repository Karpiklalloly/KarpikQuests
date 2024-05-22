using Karpik.Quests;
using Karpik.Quests.DependencyTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.Saving;
using Karpik.Quests.Tests.Quest;
using Task = Karpik.Quests.QuestSample.Task;

namespace KarpikQuestsTests.Aggregator;

public class AggregatorSerialization
{
    [Test]
    public void WhenSerializeGraph_AndDeserialize_ThenTheyAreEqual()
    {
        //Action
        IAggregator aggregator = new Karpik.Quests.QuestSample.Aggregator();
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = CreateQuest();
        var quest2 = CreateQuest();
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);
        aggregator.TryAddDependence(graph, quest2, quest1, new Completion());
        
        var serializer = new JsonResolver<IAggregator>();
        var json = serializer.Serialize(aggregator);

        //Condition
        var clone = serializer.Deserialize(json);
        
        //Result
        Assert.That(aggregator.Quests.Count() == clone.Quests.Count());
        Assert.That(EqualEnumerables(
            aggregator.GetDependencies(graph, aggregator.Quests.ElementAt(0)),
            aggregator.GetDependencies(graph, clone.Quests.ElementAt(0))));
        Assert.That(EqualEnumerables(
            aggregator.GetDependencies(graph, aggregator.Quests.ElementAt(1)),
            aggregator.GetDependencies(graph, clone.Quests.ElementAt(1))));
        Assert.That(EqualEnumerables(
            aggregator.GetDependents(graph, aggregator.Quests.ElementAt(0)),
            aggregator.GetDependents(graph, clone.Quests.ElementAt(0))));
        Assert.That(EqualEnumerables(
            aggregator.GetDependents(graph, aggregator.Quests.ElementAt(1)),
            aggregator.GetDependents(graph, clone.Quests.ElementAt(1))));
        
        for (int i = 0; i < graph.Quests.Count(); i++)
        {
            QuestSerialization.AssetFullEqual(aggregator.Quests.ElementAt(i), clone.Quests.ElementAt(i));
        }
    }

    [Test]
    public void WhenSerializeGraph_AndDeserialize_ThenCompletionWork()
    {
        //Action
        IAggregator aggregator = new Karpik.Quests.QuestSample.Aggregator();
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = CreateQuest();
        var quest2 = CreateQuest();
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);
        aggregator.TryAddDependence(graph, quest2, quest1, new Completion());
        bool flag = false;
        
        var serializer = new JsonResolver<IAggregator>();
        var json = serializer.Serialize(aggregator);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.QuestCompleted += quest => flag = true;
        foreach (var quest in clone.Quests)
        {
            foreach (var bundle in quest.TaskBundles)
            {
                foreach (var task in bundle)
                {
                    task.ForceComplete();
                }
            }
        }
        
        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerializeGraph_AndDeserialize_ThenFailedWork()
    {
        //Action
        IAggregator aggregator = new Karpik.Quests.QuestSample.Aggregator();
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = CreateQuest();
        var quest2 = CreateQuest();
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);
        aggregator.TryAddDependence(graph, quest2, quest1, new Completion());
        bool flag = false;
        
        var serializer = new JsonResolver<IAggregator>();
        var json = serializer.Serialize(aggregator);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.QuestFailed += quest => flag = true;
        foreach (var quest in clone.Quests)
        {
            foreach (var bundle in quest.TaskBundles)
            {
                foreach (var task in bundle)
                {
                    task.ForceFail();
                }
            }
        }
        
        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerializeGraph_AndDeserialize_ThenUpdatedWork()
    {
        //Action
        IAggregator aggregator = new Karpik.Quests.QuestSample.Aggregator();
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = CreateQuest();
        var quest2 = CreateQuest();
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);
        aggregator.TryAddDependence(graph, quest2, quest1, new Completion());
        bool flag = false;
        
        var serializer = new JsonResolver<IAggregator>();
        var json = serializer.Serialize(aggregator);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.QuestUpdated += quest => flag = true;
        clone.Quests.First().TaskBundles.First().Tasks.First().ForceComplete();
        
        //Result
        Assert.That(flag);
    }
    
    [Test]
    public void WhenSerializeGraph_AndDeserialize_ThenStartedWork()
    {
        //Action
        IAggregator aggregator = new Karpik.Quests.QuestSample.Aggregator();
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = CreateQuest();
        var quest2 = CreateQuest();
        aggregator.TryAddGraph(graph);
        aggregator.TryAddQuest(graph, quest1);
        aggregator.TryAddQuest(graph, quest2);
        aggregator.TryAddDependence(graph, quest2, quest1, new Completion());
        bool flag = false;
        
        var serializer = new JsonResolver<IAggregator>();
        var json = serializer.Serialize(aggregator);

        //Condition
        var clone = serializer.Deserialize(json);
        clone.QuestStarted += quest => flag = true;
        clone.Start();
        
        //Result
        Assert.That(flag);
    }
    
    public static void AssertFullEqual(IAggregator aggregator, IAggregator clone)
    {
        
    }
    
    private IQuest CreateQuest()
    {
        var bundle1 = new TaskBundle(); bundle1.Add(new Task()); bundle1.Add(new Task());
        var bundle2 = new TaskBundle(); bundle2.Add(new Task()); bundle2.Add(new Task());
        var quest = QuestBuilder.Start<Quest>("Name", "dfgfmbkltdnlkmlk ☺♣♠")
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
        return quest;
    }
    
    private static bool EqualEnumerables<T>(IEnumerable<T> first, IEnumerable<T> second)
    {
        for (int i = 0; i < first.Count(); i++)
        {
            var f = first.ElementAt(i);
            var s = second.ElementAt(i);
            if (!f.Equals(s)) return false;
        }

        return true;
    }
}