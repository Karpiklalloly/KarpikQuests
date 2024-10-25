using NewKarpikQuests;
using NewKarpikQuests.Extensions;
using NewKarpikQuests.Sample;

namespace KarpikQuestsTests.GraphTests
{
    public class AddDependency
    {
        [Test]
        public void WhenAddCompletedDependency_AndDependencyIsOk_ThenQuestIsStarted([Values(
            DependencyType.Completion,
            DependencyType.Unlocked,
            DependencyType.Fail)] DependencyType dependency)
    {
        //Action
        var graph = new Graph();
        var subQuest = new Quest();
        
        var quest1 = QuestBuilder.Start("name1", "description1")
            .SetGraph(graph)
            .AddSubQuest(subQuest)
            .Build();

        var quest2 = QuestBuilder.Start("name2", "description2")
            .SetGraph(graph)
            .Build();
        
        graph.TryAddDependency(quest2, quest1, dependency);
        
        //Condition
        switch (dependency)
        {
            case DependencyType.Completion:
                subQuest.ForceComplete();
                break;
            case DependencyType.Fail:
                subQuest.ForceFail();
                break;
            case DependencyType.Unlocked:
                subQuest.ForceUnlock();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dependency), dependency, null);
        }
        
        graph.Update(quest1.Id);

        //Result
        Assert.That(quest2.IsUnlocked());
    }
    }
}