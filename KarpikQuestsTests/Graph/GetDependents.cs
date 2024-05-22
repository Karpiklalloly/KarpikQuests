using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Graph;

public class GetDependents
{
    [Test]
    public void WhenAddDependents_AndGetDependents_ThenQuestHasTheeseDependets()
    {
        //Action
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAddDependency(quest2, quest1, IGraph.DependencyType.Completion);
        graph.TryAddDependency(quest3, quest1, IGraph.DependencyType.Completion);

        //Condition
        var dependents = graph.GetDependentsQuests(quest1).ToList();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(dependents.Find(x => x.Equals(quest2)), Is.Not.EqualTo(null));
            Assert.That(dependents.Find(x => x.Equals(quest3)), Is.Not.EqualTo(null));
        });
    }
}