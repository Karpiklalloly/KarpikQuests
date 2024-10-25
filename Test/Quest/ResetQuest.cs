using NewKarpikQuests;
using NewKarpikQuests.Extensions;

namespace KarpikQuestsTests.QuestTests
{
    public class ResetQuest
    {
        [Test]
        public void WhenCompleteSomeSubSubQuests_AndReset_ThenSubSubQuestsAreUncompleted()
    {
        //Action
        var quest = new Quest();
        var task1 = new Quest();
        var task2 = new Quest();
        var task3 = new Quest();
        var bundle = new Quest();
        bundle.Add(task1);
        bundle.Add(task2);
        bundle.Add(task3);
        quest.Add(bundle);
        quest.Setup();
        task1.TryComplete();
        task2.TryComplete();

        //Condition
        quest.ForceLock();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(task1.IsLocked());
            Assert.That(task2.IsLocked());
            Assert.That(task3.IsLocked());
        });
    }
    
        [Test]
        public void WhenCompleteAllSubSubQuests_AndReset_ThenSubSubQuestsAreUncompleted()
    {
        //Action
        var quest = new Quest();
        var task1 = new Quest();
        var task2 = new Quest();
        var task3 = new Quest();
        var bundle = new Quest();
        bundle.Add(task1);
        bundle.Add(task2);
        bundle.Add(task3);
        quest.Add(bundle);
        quest.Setup();
        task1.TryComplete();
        task2.TryComplete();
        task3.TryComplete();

        //Condition
        quest.ForceLock();
        
        //Result
        Assert.Multiple(() =>
        {
            Assert.That(quest.IsLocked);
            Assert.That(task1.IsLocked());
            Assert.That(task2.IsLocked());
            Assert.That(task3.IsLocked());
        });
    }
    }
}