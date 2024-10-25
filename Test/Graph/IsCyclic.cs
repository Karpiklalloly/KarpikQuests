using Karpik.Quests;
using Karpik.Quests.Extensions;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Sample;

namespace KarpikQuestsTests.GraphTests
{
    public class IsCyclic
    {
        [Test]
        public void WhenCreateCyclic_AndCallIsCyclic_ThenTrue()
    {
        //Action
        IGraph graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAddDependency(quest1, quest2, DependencyType.Completion);
        graph.TryAddDependency(quest2, quest3, DependencyType.Completion);
        graph.TryAddDependency(quest3, quest1, DependencyType.Completion);

        //Condition
        var result = graph.IsCyclic();

        //Result
        Assert.That(result);
    }
    
        [Test]
        public void WhenCreateCyclic_AndCallIsCyclic_ThenTrue2()
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
        graph.TryAdd(quest4);
        graph.TryAddDependency(quest2, quest1, DependencyType.Completion);
        graph.TryAddDependency(quest2, quest3, DependencyType.Completion);
        graph.TryAddDependency(quest3, quest4, DependencyType.Completion);
        graph.TryAddDependency(quest4, quest2, DependencyType.Completion);

        //Condition
        var result = graph.IsCyclic();

        //Result
        Assert.That(result);
    }
    
        [Test]
        public void WhenCreateCyclic_AndCallIsCyclic_ThenTrue3()
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
        graph.TryAdd(quest4);
        graph.TryAddDependency(quest2, quest3, DependencyType.Completion);
        graph.TryAddDependency(quest3, quest4, DependencyType.Completion);
        graph.TryAddDependency(quest4, quest2, DependencyType.Completion);

        //Condition
        var result = graph.IsCyclic();

        //Result
        Assert.That(result);
    }
    }
}