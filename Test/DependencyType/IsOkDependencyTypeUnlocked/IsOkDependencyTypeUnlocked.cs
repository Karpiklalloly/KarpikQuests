using Karpik.Quests;
using Karpik.Quests.DependencyTypes;
using Karpik.Quests.Interfaces;

namespace Test.DependencyType
{
    public class IsOkDependencyTypeUnlocked
    {
        [Test]
        public void WhenUnlocked_AndQuestIsCompleted_ThenNotOk()
    {
        //Action
        IDependencyType Unlocked = new Unlocked();
        var quest = new Quest();
        quest.ForceComplete();

        //Condition
        var result = Unlocked.IsOk(quest);

        //Result
        Assert.That(!result);
    }
    
        [Test]
        public void WhenUnlocked_AndQuestIsFailed_ThenNotOk()
    {
        //Action
        IDependencyType Unlocked = new Unlocked();
        var quest = new Quest();
        quest.ForceFail();

        //Condition
        var result = Unlocked.IsOk(quest);

        //Result
        Assert.That(!result);
    }
    
        [Test]
        public void WhenUnlocked_AndQuestIsLocked_ThenNotOk()
    {
        //Action
        IDependencyType Unlocked = new Unlocked();
        var quest = new Quest();
        quest.ForceLock();

        //Condition
        var result = Unlocked.IsOk(quest);

        //Result
        Assert.That(!result);
    }
    
        [Test]
        public void WhenUnlocked_AndQuestIsUnlocked_ThenOk()
    {
        //Action
        IDependencyType Unlocked = new Unlocked();
        var quest = new Quest();
        quest.ForceUnlock();

        //Condition
        var result = Unlocked.IsOk(quest);

        //Result
        Assert.That(result);
    }   
    }
}