using Karpik.Quests;
using Karpik.Quests.Extensions;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Task = Karpik.Quests.QuestSample.Task;

namespace KarpikQuestsTests.Graph;

public class AddDependency
{
    [Test]
    public void WhenAddCompletedDependency_AndDependencyIsOk_ThenQuestIsStarted([Values(
        IGraph.DependencyType.Completion,
        IGraph.DependencyType.Start,
        IGraph.DependencyType.Fail,
        IGraph.DependencyType.Unneccesary)] IGraph.DependencyType dependency)
    {
        //Action
        var graph = new QuestGraph();
        var task1 = new Task();
        var task2 = new Task();
        var quest1 = QuestBuilder.Start<Quest>("name1", "description1")
            .AddTask(task1)
            .SetGraph(graph)
            .Build();

        var quest2 = QuestBuilder.Start<Quest>("name2", "description2")
            .AddTask(task2)
            .SetGraph(graph)
            .Build();
        
        graph.TryAdd(quest1);
        graph.TryAdd(quest2);
        graph.TryAddDependency(quest2, quest1, dependency);
        
        //Condition
        switch (dependency)
        {
            case IGraph.DependencyType.Completion:
                quest1.Start();
                task1.ForceComplete();
                break;
            case IGraph.DependencyType.Fail:
                quest1.Start();
                task1.ForceFail();
                break;
            case IGraph.DependencyType.Start:
                quest1.Start();
                break;
            case IGraph.DependencyType.Unneccesary:
                break;
        }
        task1.ForceComplete();

        //Result
        Assert.That(quest2.IsStarted());
    }
}