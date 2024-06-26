﻿using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace KarpikQuestsTests.Graph;

public class IsCyclic
{
    [Test]
    public void WhenCreateCyclic_AndCallIsCyclic_ThenTrue()
    {
        //Action
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAddDependency(quest1, quest2, IGraph.DependencyType.Completion);
        graph.TryAddDependency(quest2, quest3, IGraph.DependencyType.Completion);
        graph.TryAddDependency(quest3, quest1, IGraph.DependencyType.Completion);

        //Condition
        var result = graph.IsCyclic();

        //Result
        Assert.That(result);
    }
    
    [Test]
    public void WhenCreateCyclic_AndCallIsCyclic_ThenTrue2()
    {
        //Action
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        var quest4 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAdd(quest4);
        graph.TryAddDependency(quest2, quest1, IGraph.DependencyType.Completion);
        graph.TryAddDependency(quest2, quest3, IGraph.DependencyType.Completion);
        graph.TryAddDependency(quest3, quest4, IGraph.DependencyType.Completion);
        graph.TryAddDependency(quest4, quest2, IGraph.DependencyType.Completion);

        //Condition
        var result = graph.IsCyclic();

        //Result
        Assert.That(result);
    }
    
    [Test]
    public void WhenCreateCyclic_AndCallIsCyclic_ThenTrue3()
    {
        //Action
        IGraph graph = new Karpik.Quests.QuestSample.Graph();
        var quest1 = new Quest();
        var quest2 = new Quest();
        var quest3 = new Quest();
        var quest4 = new Quest();
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAdd(quest3);
        graph.TryAdd(quest4);
        graph.TryAddDependency(quest2, quest3, IGraph.DependencyType.Completion);
        graph.TryAddDependency(quest3, quest4, IGraph.DependencyType.Completion);
        graph.TryAddDependency(quest4, quest2, IGraph.DependencyType.Completion);

        //Condition
        var result = graph.IsCyclic();

        //Result
        Assert.That(result);
    }
}