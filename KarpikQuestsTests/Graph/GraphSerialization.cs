using Karpik.Quests;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.Saving;
using Karpik.Quests.Tests.Quest;
using Task = Karpik.Quests.QuestSample.Task;

namespace KarpikQuestsTests.Graph;

public class GraphSerialization
{
    [Test]
    public void WhenSerializeGraph_AndDeserialize_ThenTheyAreEqual()
    {
        //Action
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = CreateQuest();
        var quest2 = CreateQuest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAddDependency(quest2, quest1, IGraph.DependencyType.Completion);
        
        var serializer = new JsonResolver<IGraph>();
        var json = serializer.Serialize(graph);

        //Condition
        var clone = serializer.Deserialize(json);

        //Result
        AssertFullEqual(graph, clone);
    }

    public static void AssertFullEqual(IGraph graph, IGraph clone)
    {
        Assert.That(graph.Quests.Count() == clone.Quests.Count());
        Assert.That(graph.StartQuests.Count() == clone.StartQuests.Count());
        Assert.That(EqualEnumerables(graph.GetDependenciesQuests(graph.Quests.ElementAt(0)), clone.GetDependenciesQuests(graph.Quests.ElementAt(0))));
        Assert.That(EqualEnumerables(graph.GetDependenciesQuests(graph.Quests.ElementAt(1)), clone.GetDependenciesQuests(graph.Quests.ElementAt(1))));
        Assert.That(EqualEnumerables(graph.GetDependentsQuests(graph.Quests.ElementAt(0)), clone.GetDependentsQuests(graph.Quests.ElementAt(0))));
        Assert.That(EqualEnumerables(graph.GetDependentsQuests(graph.Quests.ElementAt(1)), clone.GetDependentsQuests(graph.Quests.ElementAt(1))));
        
        for (int i = 0; i < graph.Quests.Count(); i++)
        {
            QuestSerialization.AssetFullEqual(graph.Quests.ElementAt(i), clone.Quests.ElementAt(i));
        }

        for (int i = 0; i < graph.StartQuests.Count(); i++)
        {
            QuestSerialization.AssetFullEqual(graph.StartQuests.ElementAt(i), clone.StartQuests.ElementAt(i));
        }
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