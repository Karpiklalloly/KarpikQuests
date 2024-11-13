using Karpik.Quests;

namespace KarpikQuestsTests.GraphTests
{
    public class GetDependents
    {
        [Test]
        public void WhenAddDependents_AndGetDependents_ThenQuestHasTheeseDependets()
    {
        //Action
        IGraph graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAddDependency(quest2, quest1, DependencyType.Completion);
        graph.TryAddDependency(quest3, quest1, DependencyType.Completion);

        //Condition
        var dependents = graph.GetDependentsQuests(quest1).ToList();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(dependents.Find(x => x.DependentQuest.Equals(quest2)), Is.Not.EqualTo(null));
            Assert.That(dependents.Find(x => x.DependentQuest.Equals(quest3)), Is.Not.EqualTo(null));
        });
    }
    }
}