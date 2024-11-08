using Karpik.Quests;
using Karpik.Quests.DependencyTypes;

namespace Test.DependencyType
{
    public class IsOkDependencyTypeFail
    {
        [Test]
        public void WhenFail_AndQuestIsCompleted_ThenNotOk()
    {
        //Action
        IDependencyType Fail = new Fail();
        var quest = new Quest();
        quest.ForceComplete();

        //Condition
        var result = Fail.IsOk(quest);

        //Result
        Assert.That(!result);
    }
    
        [Test]
        public void WhenFail_AndQuestIsFailed_ThenOk()
    {
        //Action
        IDependencyType Fail = new Fail();
        var quest = new Quest();
        quest.ForceFail();

        //Condition
        var result = Fail.IsOk(quest);

        //Result
        Assert.That(result);
    }
    
        [Test]
        public void WhenFail_AndQuestIsLocked_ThenNotOk()
    {
        //Action
        IDependencyType Fail = new Fail();
        var quest = new Quest();
        quest.ForceLock();

        //Condition
        var result = Fail.IsOk(quest);

        //Result
        Assert.That(!result);
    }
    
        [Test]
        public void WhenFail_AndQuestIsUnlocked_ThenNotOk()
    {
        //Action
        IDependencyType Fail = new Fail();
        var quest = new Quest();
        quest.ForceUnlock();

        //Condition
        var result = Fail.IsOk(quest);

        //Result
        Assert.That(!result);
    }
    }
}