using Karpik.Quests;

namespace KarpikQuestsTests.GraphTests
{
    public class Replace
    {
        [Test]
        public void WhenAddQuest_AndReplaceWithAnother_ThenItIsReplaced()
    {
        //Action
        IGraph graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        var quest4 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAddDependency(quest2, quest1, DependencyType.Completion);
        graph.TryAddDependency(quest3, quest2, DependencyType.Completion);

        //Condition
        graph.TryReplace(quest2, quest4);

        //Result
        var dependencies = graph.GetDependenciesQuests(quest4).ToList();
        var dependents = graph.GetDependentsQuests(quest4).ToList();
        Assert.Multiple(() =>
        {
            Assert.That(dependencies, Has.Count.EqualTo(1));
            Assert.That(dependencies.FindIndex(x => x.DependencyQuest.Equals(quest1)), Is.GreaterThanOrEqualTo(0));
            Assert.That(dependents, Has.Count.EqualTo(1));
            Assert.That(dependents.FindIndex(x => x.DependentQuest.Equals(quest3)), Is.GreaterThanOrEqualTo(0));
        });
    }
    }
}