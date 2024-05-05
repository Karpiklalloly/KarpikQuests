using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Graph;

public class SetDependency
{
    [Test]
    public void WhenAddQuests_AndSetDependencies_ThenRightDependencies()
    {
        //Action
        IGraph graph = new QuestGraph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);

        //Condition
        graph.TryAddDependency(quest1, quest2, IGraph.DependencyType.Completion);
        graph.TryAddDependency(quest1, quest3, IGraph.DependencyType.Completion);

        //Result
        var dependencies = graph.GetDependenciesQuests(quest1).ToList();
        var d2 = graph.GetDependenciesQuests(quest2);
        var d3 = graph.GetDependenciesQuests(quest2);
        
        Assert.Multiple(() =>
        {
            Assert.That(dependencies.Find(x => x.Equals(quest2)), Is.Not.EqualTo(null));
            Assert.That(dependencies.Find(x => x.Equals(quest3)), Is.Not.EqualTo(null));
            Assert.That(!d2.Any());
            Assert.That(!d3.Any());
        });
    }
}