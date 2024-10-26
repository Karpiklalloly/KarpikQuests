using Karpik.Quests;
using Karpik.Quests.Extensions;

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

        var quest1 = new Quest("name1", "description1");
        quest1.Add(subQuest);
        graph.TryAdd(quest1);

        var quest2 = new Quest("name2", "description2");
        graph.TryAdd(quest2);
        
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

        //Result
        Assert.That(quest2.IsUnlocked());
    }
    }
}