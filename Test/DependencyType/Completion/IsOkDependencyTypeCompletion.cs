using NewKarpikQuests;
using NewKarpikQuests.DependencyTypes;
using NewKarpikQuests.Interfaces;

namespace Test.DependencyType
{
    public class IsOkDependencyTypeCompletion
    {
        [Test]
        public void WhenCompletion_AndQuestIsCompleted_ThenOk()
    {
        //Action
        IDependencyType completion = new Completion();
        var quest = new Quest();
        quest.ForceComplete();

        //Condition
        var result = completion.IsOk(quest);

        //Result
        Assert.That(result);
    }
    
        [Test]
        public void WhenCompletion_AndQuestIsFailed_ThenNotOk()
    {
        //Action
        IDependencyType completion = new Completion();
        var quest = new Quest();
        quest.ForceFail();

        //Condition
        var result = completion.IsOk(quest);

        //Result
        Assert.That(!result);
    }
    
        [Test]
        public void WhenCompletion_AndQuestIsLocked_ThenNotOk()
    {
        //Action
        IDependencyType completion = new Completion();
        var quest = new Quest();
        quest.ForceLock();

        //Condition
        var result = completion.IsOk(quest);

        //Result
        Assert.That(!result);
    }
    
        [Test]
        public void WhenCompletion_AndQuestIsUnlocked_ThenNotOk()
    {
        //Action
        IDependencyType completion = new Completion();
        var quest = new Quest();
        quest.ForceUnlock();

        //Condition
        var result = completion.IsOk(quest);

        //Result
        Assert.That(!result);
    }
    }
}