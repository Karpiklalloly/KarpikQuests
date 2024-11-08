using Karpik.Quests;
using Karpik.Quests.Extensions;

namespace KarpikQuestsTests.GraphTests
{
    public class RemoveDependenciesGraph
    {
        [Test]
        public void WhenAddDependencies_AndTryRemoveThem_ThenThereAreNoDependencies()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAddDependency(quest3, quest1, DependencyType.Completion);
        graph.TryAddDependency(quest3, quest2, DependencyType.Completion);

        //Condition
        graph.TryRemoveDependencies(quest3);
        
        //Result
        Assert.That(!graph.GetDependenciesQuests(quest3).Any());
    }
    
        [Test]
        public void WhenAddDependency_AndTryRemoveIt_ThenThereIsNoDependency()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAddDependency(quest2, quest1, DependencyType.Completion);

        //Condition
        graph.TryRemoveDependency(quest2.Id, quest1.Id);
        
        //Result
        Assert.That(!graph.GetDependenciesQuests(quest2).Any());
    }
    
        [Test]
        public void WhenAddDependencies_AndTryRemoveThem_ThenThereAreNoDependents()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAddDependency(quest3, quest1, DependencyType.Completion);
        graph.TryAddDependency(quest3, quest2, DependencyType.Completion);

        //Condition
        graph.TryRemoveDependencies(quest3);
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(!graph.GetDependentsQuests(quest1).Any());
            Assert.That(!graph.GetDependentsQuests(quest2).Any());
        });
    }

        [Test]
        public void WhenAddDependency_AndTryRemoveIt_ThenThereIsNoDependent()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAddDependency(quest2, quest1, DependencyType.Completion);

        //Condition
        graph.TryRemoveDependency(quest2.Id, quest1.Id);
        
        //Result
        Assert.That(!graph.GetDependentsQuests(quest1).Any());
    }
    
        [Test]
        public void WhenAddDependencies_AndTryRemoveDependents_ThenThereAreNoDependents()
    {
        //Action
        var graph = new Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAddDependency(quest3, quest1, DependencyType.Completion);
        graph.TryAddDependency(quest3, quest2, DependencyType.Completion);

        //Condition
        graph.TryRemoveDependents(quest1);
        graph.TryRemoveDependents(quest2);
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(!graph.GetDependentsQuests(quest1).Any());
            Assert.That(!graph.GetDependentsQuests(quest2).Any());
            Assert.That(!graph.GetDependenciesQuests(quest3).Any());
        });
    }
    }
}